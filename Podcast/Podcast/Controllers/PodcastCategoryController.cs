using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

            IEnumerable<PodcastAdminVM> podcasts;

            if (sortOrder == "most_followed")
            {
                podcasts = await _podcastService.GetAllByCategorySortedByFollowCountShowMoreAsync(podcastCategory.Id, 0, 8);
            }
            else if (sortOrder == "less_followed")
            {
                podcasts = await _podcastService.GetAllByCategorySortedByFollowCountLessShowMoreAsync(podcastCategory.Id, 0, 8);
            }
            else if (sortOrder == "oldest")
            {
                podcasts = await _podcastService.GetAllByCategorySortedByOldestAsync(podcastCategory.Id, 0, 8);
            }
            else
            {
                podcasts = await _podcastService.GetAllByCategoryShowMoreAsync(id);
            }

            var followedPodcastIds = userId != null
                ? await _appUserPodcastService.GetFollowedPodcastIdsAsync(userId)
                : new List<int>();

            var allPodcasts = await _podcastService.GetAllByCategoryAsync(podcastCategory.Id);
            var podcastCount=allPodcasts.Count();

            var viewModel = new PodcastCategoryVM
            {
                Podcasts = podcasts,
                PodcastCategory = podcastCategory,
                FollowedPodcastIds = followedPodcastIds,
                PodcastCount = podcastCount,
                SortOrder = sortOrder
            };


            return View(viewModel);
        }
        public async Task<IActionResult> ShowMore(int id, int skip, string sortOrder = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var podcastCategory = await _podcastCategoryService.GetByIdAsync(id);

            IEnumerable<PodcastAdminVM> podcasts;

            if (sortOrder == "most_followed")
            {
                podcasts = await _podcastService.GetAllByCategorySortedByFollowCountShowMoreAsync(podcastCategory.Id, skip, 4);
            }
            else if (sortOrder == "less_followed")
            {
                podcasts = await _podcastService.GetAllByCategorySortedByFollowCountLessShowMoreAsync(podcastCategory.Id, skip, 4);
            }
            else if (sortOrder == "oldest")
            {
                podcasts = await _podcastService.GetAllByCategorySortedByOldestAsync(podcastCategory.Id, skip, 4);
            }
            else
            {
                podcasts = await _podcastService.GetAllByCategoryShowMoreAsync(id, skip, 4);
            }

            var followedPodcastIds = userId != null
                ? await _appUserPodcastService.GetFollowedPodcastIdsAsync(userId)
                : new List<int>();

            var viewModel = new PodcastCategoryVM
            {
                Podcasts = podcasts,
                PodcastCategory = podcastCategory,
                FollowedPodcastIds = followedPodcastIds
            };

            return PartialView("_PodcastPartial", viewModel);
        }


    }
}
