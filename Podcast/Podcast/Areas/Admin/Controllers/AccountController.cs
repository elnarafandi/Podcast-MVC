using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace Podcast.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    await _accountService.CreateRolesAsync();
        //    return Ok();
        //}
    }
}
