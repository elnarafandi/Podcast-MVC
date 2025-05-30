using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Guest;
using Service.ViewModels.TeamMember;

namespace Podcast.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GuestController : Controller
    {
        private readonly IGuestService _guestService;
        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var guests = await _guestService.GetAllAsync();
            return View(guests);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuestCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);
            await _guestService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var guest = await _guestService.GetByIdAsync(id);
            return View(new GuestEditVM
            {
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Image = guest.Image,
                Information = guest.Information,
                SocialMedia = guest.SocialMedia
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GuestEditVM request)
        {
            await _guestService.EditAsync(id, request);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _guestService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var guest = await _guestService.GetByIdAsync(id);
            return View(guest);
        }
    }
}
