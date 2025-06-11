using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Service.Services.Interfaces;
using Service.ViewModels.Account;
using Service.ViewModels.TeamMember;

namespace Podcast.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(IAccountService accountService,
                                 UserManager<AppUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
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
            await _accountService.RegisterAsync(model);
            return RedirectToAction(nameof(Login));
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
            return View(new AccountEditVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Image = user.Image
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(string id, AccountEditVM request)
        {
            await _accountService.EditAccountAsync(id, request);
            return RedirectToAction("Index", "Home");
        }

    }
}
