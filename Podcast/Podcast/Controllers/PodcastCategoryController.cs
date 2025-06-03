using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.PodcastCategory;

namespace Podcast.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class PodcastCategoryController : Controller
    {
        private readonly IPodcastCategoryService _podcastCategoryService;
        private readonly IPodcastService _podcastService;
        public PodcastCategoryController(IPodcastCategoryService podcastCategoryService,
                                         IPodcastService podcastService)
        {
            _podcastCategoryService = podcastCategoryService;
            _podcastService = podcastService;
        }
        public async Task<IActionResult> Index(int id)
        {
            var podcastCategory = await _podcastCategoryService.GetByIdAsync(id);
            var podcasts= await _podcastService.GetAllByCategoryAsync(id);
            return View(new PodcastCategoryVM
            {
                Podcasts = podcasts,
                PodcastCategory=podcastCategory
            });
        }
       
    }
}
