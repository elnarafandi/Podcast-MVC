using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Home;
using Service.ViewModels.Podcast;
using System.Diagnostics;
using System.Security.Claims;

namespace Podcast.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class HomeController : Controller
    {
        private readonly IPodcastCategoryService _podcastCategoryService;
        private readonly IPodcastService _podcastService;
        private readonly IAppUserPodcastService _appUserPodcastService;
        public HomeController(IPodcastCategoryService podcastCategoryService, 
                              IPodcastService podcastService,
                              IAppUserPodcastService appUserPodcastService)
        {
            _podcastCategoryService = podcastCategoryService;
            _podcastService = podcastService;
            _appUserPodcastService = appUserPodcastService;
        }
        public async Task<IActionResult> Index()
        {
            var categories= await _podcastCategoryService.GetAllAsync();
            var podcasts= await _podcastService.GetAllAsync();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var followedPodcastIds = userId != null
                ? await _appUserPodcastService.GetFollowedPodcastIdsAsync(userId)
                : new List<int>();
            var followedPodcasts = await _appUserPodcastService.GetFollowedPodcastsAsync(userId);
            return View(new HomeVM
            {
                PodcastCategories = categories,
                Podcasts = podcasts,
                FollowedPodcastIds = followedPodcastIds,
                FollowedPodcasts = followedPodcasts
            });
        }
        
    }
}
