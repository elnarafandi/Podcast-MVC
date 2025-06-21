using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Service.Helpers.Extensions;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Account;
using Service.ViewModels.TeamMember;
using System.Net;
using System.Security.Claims;

namespace Podcast.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        public AccountController(IAccountService accountService,
                                 UserManager<AppUser> userManager,
                                 IEmailService emailService)
        {
            _accountService = accountService;
            _userManager = userManager;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.RegisterAsync(model);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code.Contains("Email"))
                    {
                        ModelState.AddModelError("Email", error.Description);
                    }
                    else if (error.Code.Contains("UserName"))
                    {
                        ModelState.AddModelError("UserName", error.Description);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View(model);
            }

            return RedirectToAction(nameof(EmailConfirmationSent));
        }



        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Invalid confirmation link.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var decodedToken = WebUtility.UrlDecode(token);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                return BadRequest("Email confirmation failed.");
            }

            return RedirectToAction(nameof(Login));
        }



        [HttpGet]
        public IActionResult EmailConfirmationSent()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Generate password reset token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Generate the reset password link
                    var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

                    // Create the email body
                    var message = $"Click <a href=\"{resetLink}\">here</a> to reset your password.";

                    // Send the email
                    await _emailService.SendEmailAsync(model.Email, "Password Reset Request", message);
                }

                // Return confirmation view, even if email is not found
                return View("EmailConfirmationSent");
            }

            // If validation failed, return the same view with validation errors
            return View(model);
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ForgotPassword");
            }

            var model = new ResetPasswordVM
            {
                Token = token,
                Email = email
            };

            return View(model);
        }

        // POST: ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var success = await _accountService.LoginAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", "Email or password is incorrect.");
                return View(model);
            }
            

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index", "About");
        }
        [HttpGet]
        public async Task<IActionResult> EditAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = roles.Contains("Admin");
            return View(new AccountEditVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Image = user.Image,
                IsAdmin = isAdmin,
                UserName=user.UserName,
                Email=user.Email
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(string id, AccountEditVM request)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = roles.Contains("Admin");
            request.Image = user.Image;
            request.IsAdmin = isAdmin;
            request.UserName = user.UserName;
            request.Email = user.Email;

            if (request.UploadImage != null)
            {
                if (!request.UploadImage.CheckFileType("image"))
                {
                    ModelState.AddModelError("UploadImage", "Only image files are allowed.");
                }

                if (!request.UploadImage.CheckFileSize(1024)) // 1 MB
                {
                    ModelState.AddModelError("UploadImage", "Image size must be less than 1 MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }
            try
            {
                await _accountService.EditAccountAsync(id, request);
                return RedirectToAction("Index", "Home");
            }
            catch (InvalidOperationException ex)
            {
                // If an error occurs, add it to ModelState and return to the same view
                ModelState.AddModelError("ConfirmPassword", ex.Message);
                

                // Return the view with the error message and existing user data
                return View(request);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "User not found. Please log in.");
                return RedirectToAction("Login", "Account"); 
            }

            var result = await _accountService.DeleteAccountAsync(userId);

            if (result.Succeeded)
            {
                await _accountService.LogoutAsync();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting your account.");
                return View(); 
            }
        }



    }
}
