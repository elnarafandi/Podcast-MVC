using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Podcast;
using System.Security.Claims;

namespace Podcast.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class PodcastController : Controller
    {
        private readonly IPodcastService _podcastService;
        private readonly IPodcastCategoryService _podcastCategoryService;
        private readonly IAppUserPodcastService _appUserPodcastService;
        public PodcastController(IPodcastService podcastService, 
                                 IPodcastCategoryService podcastCategoryService,
                                 IAppUserPodcastService appUserPodcastService)
        {
            _podcastService = podcastService;
            _podcastCategoryService = podcastCategoryService;
            _appUserPodcastService = appUserPodcastService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            var podcast= await _podcastService.GetByIdAsync(id);
            if (podcast == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isFollowing = false;
            if (!string.IsNullOrEmpty(userId))
            {
                isFollowing = await _appUserPodcastService.IsFollowingAsync(userId, id);
            }

            return View(new PodcastDetailVM
            {
                Podcast = podcast,
                IsFollowing = isFollowing
            });
        }
        [HttpGet]
        public async Task<IActionResult> Search(string podcastTitle)
        {
            if (string.IsNullOrEmpty(podcastTitle))
            {
                return BadRequest("Podcast title is required.");
            }
            var podcasts= await _podcastService.SearchByTitleAsync(podcastTitle);
            
            var podcastCategories= await _podcastCategoryService.GetAllAsync();
            return View(new PodcastVM
            {
                Podcasts = podcasts,
                PodcastCategories = podcastCategories,
                SearchText= podcastTitle
            });
        }
        [HttpGet]
        public async Task<IActionResult> Filter(string category, string searchText)
        {
            if (string.IsNullOrEmpty(category))
            {
                return BadRequest("Category is required.");
            }

            var podcasts = await _podcastService.FilterByCategoryAsync(searchText,category);
            var podcastCategories = await _podcastCategoryService.GetAllAsync();

            return View("Search", new PodcastVM
            {
                Podcasts = podcasts,
                PodcastCategories = podcastCategories,
                SearchText = searchText
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFollow(int podcastId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            bool isFollowing = await _appUserPodcastService.IsFollowingAsync(userId, podcastId);

            if (isFollowing)
                await _appUserPodcastService.UnfollowAsync(userId, podcastId);
            else
                await _appUserPodcastService.FollowAsync(userId, podcastId);

            return RedirectToAction("Detail", new { id = podcastId });
        }
    }
}
