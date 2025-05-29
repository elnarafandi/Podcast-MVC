using Azure.Core;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Helpers.Enums;
using Service.Services.Interfaces;
using Service.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AccountService:IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountService(UserManager<AppUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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

        public async Task<bool> LoginAsync(LoginVM model)
        {
            var existUser = await _userManager.FindByEmailAsync(model.UserNameOrEmail)
                  ?? await _userManager.FindByNameAsync(model.UserNameOrEmail);

            if (existUser == null)
                return false;

            var result = await _signInManager.PasswordSignInAsync(existUser, model.Password, false, false);

            return result.Succeeded;

        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task RegisterAsync(RegisterVM model)
        {
            AppUser appUser = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Image = "person2.jpg",
            };
            var result = await _userManager.CreateAsync(appUser, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            await _userManager.AddToRoleAsync(appUser, Roles.Member.ToString());
        }
    }
}
