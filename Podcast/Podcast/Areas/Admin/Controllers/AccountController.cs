using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Helpers.Enums;
using Service.Services.Interfaces;
using Service.ViewModels.Account;

namespace Podcast.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            List<UserRolesVM> userWithRoles = new();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add(new UserRolesVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email=user.Email,
                    Roles = userRoles.ToList()
                });
            }

            return View(userWithRoles);
        }

        [HttpPost]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (!await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()))
            {
                await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()))
            {
                await _userManager.RemoveFromRoleAsync(user, Roles.Admin.ToString());
            }

            return RedirectToAction(nameof(Index));
        }


        //[HttpGet]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    await _accountService.CreateRolesAsync();
        //    return Ok();
        //}
    }
}
