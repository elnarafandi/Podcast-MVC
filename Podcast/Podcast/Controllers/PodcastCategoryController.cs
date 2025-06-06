using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Podcast;
using Service.ViewModels.PodcastCategory;
using System.Security.Claims;

namespace Podcast.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class PodcastCategoryController : Controller
    {
        private readonly IPodcastCategoryService _podcastCategoryService;
        private readonly IPodcastService _podcastService;
        private readonly IAppUserPodcastService _appUserPodcastService;
        public PodcastCategoryController(IPodcastCategoryService podcastCategoryService,
                                         IPodcastService podcastService,
                                         IAppUserPodcastService appUserPodcastService)
        {
            _podcastCategoryService = podcastCategoryService;
            _podcastService = podcastService;
            _appUserPodcastService = appUserPodcastService;
        }
        public async Task<IActionResult> Index(int id, string sortOrder = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var podcastCategory = await _podcastCategoryService.GetByIdAsync(id);

            var podcasts = sortOrder == "most_followed"
                ? await _podcastService.GetAllByCategorySortedByFollowCountAsync(id)
                : await _podcastService.GetAllByCategoryAsync(id);

            var followedPodcastIds = userId != null
                ? await _appUserPodcastService.GetFollowedPodcastIdsAsync(userId)
                : new List<int>();

            var viewModel = new PodcastCategoryVM
            {
                Podcasts = podcasts,
                PodcastCategory = podcastCategory,
                FollowedPodcastIds = followedPodcastIds
            };

            return View(viewModel);
        }

    }
}
