using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace Podcast.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class PackageController : Controller
    {
        private readonly IPackageService _packageService;
        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }
        public async Task<IActionResult> Index()
        {
            var package = await _packageService.GetByIdAsync(3);
            return View(package);
        }
    }
}
