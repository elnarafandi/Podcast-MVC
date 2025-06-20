using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Podcast.Controllers;
using Service.Helpers.Extensions;
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
            // Şəkil yoxlaması
            if (request.UploadImage == null)
            {
                ModelState.AddModelError("UploadImage", "Please upload an image.");
            }
            else
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
                await _guestService.CreateAsync(request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(request);
            }

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
                Email = guest.Email,
                Information = guest.Information,
                SocialMedia = guest.SocialMedia
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GuestEditVM request)
        {
            var existingGuest = await _guestService.GetByIdAsync(id);
            request.Image = existingGuest.Image;


            // Fayl yoxlamaları
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
                await _guestService.EditAsync(id, request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(request);
            }

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
