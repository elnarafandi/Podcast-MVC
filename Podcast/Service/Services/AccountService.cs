using Azure.Core;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Service.Helpers.Enums;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AccountService:IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        public AccountService(UserManager<AppUser> userManager,
                              IEmailService emailService,
                              LinkGenerator linkGenerator,
                              IHttpContextAccessor httpContextAccessor,
                              RoleManager<IdentityRole> roleManager,
                              SignInManager<AppUser> signInManager,
                              IWebHostEnvironment env)
        {
            _userManager = userManager;
            _emailService = emailService;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _env = env;
        }

        public async Task CreateRolesAsync()
        {
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
        }

        public async Task EditAccountAsync(string id, AccountEditVM request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (request.UploadImage != null)
            {
                if (!request.UploadImage.CheckFileType("image"))
                    throw new InvalidOperationException("Only image files are allowed.");
                if (!request.UploadImage.CheckFileSize(1024)) // 1 MB limit
                    throw new InvalidOperationException("Image size should be less than 1 MB.");

                if (!string.IsNullOrEmpty(user.Image))
                {
                    string oldFilePath = _env.GenerateFilePath("assets/images/home", user.Image);
                    oldFilePath.DeleteFile();
                }
                    
                string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
                string filePath = _env.GenerateFilePath("assets/images/home", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }
                user.Image = fileName;
            }
            user.FirstName=request.FirstName;
            user.LastName=request.LastName;

            if (!string.IsNullOrEmpty(request.Password) && !string.IsNullOrEmpty(request.ConfirmPassword))
            {
                if (request.Password != request.ConfirmPassword)
                {
                    throw new InvalidOperationException("Password and Confirm Password do not match.");
                }

                var hasPassword = await _userManager.HasPasswordAsync(user);
                if (hasPassword)
                {
                    var removeResult = await _userManager.RemovePasswordAsync(user);

                }

                var addResult = await _userManager.AddPasswordAsync(user, request.Password);
            }
            

            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> LoginAsync(LoginVM model)
        {
            var existUser = await _userManager.FindByEmailAsync(model.UserNameOrEmail)
                  ?? await _userManager.FindByNameAsync(model.UserNameOrEmail);

            if (existUser == null)
                return false;

            var result = await _signInManager.PasswordSignInAsync(existUser, model.Password, model.RememberMe, false);

            return result.Succeeded;

        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterVM model)
        {
            AppUser appUser = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                PackageId = 4,
                PurchasedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(appUser, model.Password);
            if (!result.Succeeded)
            {
                return result;
            }

            await _userManager.AddToRoleAsync(appUser, Roles.Member.ToString());

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var encodedToken = WebUtility.UrlEncode(token);
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                throw new Exception("HttpContext is null.");


            var confirmationLink = _linkGenerator.GetUriByAction(
                httpContext: _httpContextAccessor.HttpContext,
                action: "ConfirmEmail",
                controller: "Account",
                values: new { userId = appUser.Id, token = encodedToken },
                scheme: request.Scheme
            );


            string message = $"Zəhmət olmasa e-poçtunuzu təsdiqləyin: <a href='{confirmationLink}'>Təsdiqlə</a>";
            await _emailService.SendEmailAsync(appUser.Email, "Email təsdiqi", message);

            return IdentityResult.Success;
        }


        public async Task<IdentityResult> DeleteAccountAsync(string userId)
        {
            
            var currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            if (!string.IsNullOrEmpty(currentUser.Image))
            {
                string filePath = _env.GenerateFilePath("assets/images/home", currentUser.Image);
                filePath.DeleteFile();
            }

            
            var result = await _userManager.DeleteAsync(currentUser);

            return result;
        }





    }
}
