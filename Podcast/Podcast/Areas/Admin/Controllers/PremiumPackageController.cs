using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Guest;
using Service.ViewModels.Package;

namespace Podcast.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PremiumPackageController : Controller
    {
        private readonly IPackageService _packageService;
        public PremiumPackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var packages= await _packageService.GetAllAsync();
            return View(packages);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var package= await _packageService.GetByIdAsync(id);
            return View(new PackageEditVM
            {
                Price = package.Price
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PackageEditVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            await _packageService.EditAsync(id, request);
            return RedirectToAction(nameof(Index));
        }
    }
}
