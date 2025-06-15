using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
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
        private readonly IPlaylistService _playlistService;
        private readonly UserManager<AppUser> _userManager;
        public HomeController(IPodcastCategoryService podcastCategoryService, 
                              IPodcastService podcastService,
                              IAppUserPodcastService appUserPodcastService,
                              IPlaylistService playlistService,
                              UserManager<AppUser> userManager)
        {
            _podcastCategoryService = podcastCategoryService;
            _podcastService = podcastService;
            _appUserPodcastService = appUserPodcastService;
            _playlistService = playlistService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 6;


            var categories= await _podcastCategoryService.GetAllAsync();

            var podcasts = await _podcastService.GetAllAsync();
            var pagedPodcasts = podcasts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var totalPodcasts = podcasts.Count;
            var totalPages = (int)Math.Ceiling((double)totalPodcasts / pageSize);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var playlists = await _playlistService.GetPlaylistsByUserIdAsync(userId);
            var followedPodcastIds = userId != null
                ? await _appUserPodcastService.GetFollowedPodcastIdsAsync(userId)
                : new List<int>();
            var followedPodcasts = await _appUserPodcastService.GetFollowedPodcastsAsync(userId);

            

            var model = new HomeVM
            {
                PodcastCategories = categories,
                Podcasts = pagedPodcasts,
                FollowedPodcastIds = followedPodcastIds,
                FollowedPodcasts = followedPodcasts,
                Playlists = playlists,
                CurrentPage = page,
                TotalPages = totalPages,
                User=user
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_PodcastHomePartial", model);
            }


                return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create-playlist")]
        public async Task<IActionResult> CreatePlaylist(string playlist_name)
        {
            if (string.IsNullOrWhiteSpace(playlist_name))
            {
                ModelState.AddModelError("", "dont playlsit");
                return RedirectToAction("Index", "Home"); 
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); 
            }

            var playlist = new Playlist
            {
                Name = playlist_name,
                AppUserId = userId
            };

            await _playlistService.CreatePlaylistAsync(playlist);

            return RedirectToAction("Index", "Home"); 
        }
    }
}
