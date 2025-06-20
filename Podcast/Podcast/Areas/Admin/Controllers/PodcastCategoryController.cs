using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.PodcastCategory;

namespace Podcast.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PodcastCategoryController : Controller
    {
        private readonly IPodcastCategoryService _podcastCategoryService;

        public PodcastCategoryController(IPodcastCategoryService podcastCategoryService)
        {
            _podcastCategoryService = podcastCategoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var podcastCategories= await _podcastCategoryService.GetAllAsync();
            return View(podcastCategories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PodcastCategoryCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);
            try
            {
                await _podcastCategoryService.CreateAsync(request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(request);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var podcastCategory = await _podcastCategoryService.GetByIdAsync(id);
            return View(new PodcastCategoryEditVM {Name=podcastCategory.Name});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PodcastCategoryEditVM request)
        {
            if (!ModelState.IsValid) return View(request);

            try
            {
                await _podcastCategoryService.EditAsync(id, request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(request);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _podcastCategoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        

    }
}
