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
        public HomeController(IPodcastCategoryService podcastCategoryService, 
                              IPodcastService podcastService,
                              IAppUserPodcastService appUserPodcastService,
                              IPlaylistService playlistService)
        {
            _podcastCategoryService = podcastCategoryService;
            _podcastService = podcastService;
            _appUserPodcastService = appUserPodcastService;
            _playlistService = playlistService;
        }
        public async Task<IActionResult> Index()
        {
            var categories= await _podcastCategoryService.GetAllAsync();
            var podcasts= await _podcastService.GetAllAsync();
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var playlists = await _playlistService.GetPlaylistsByUserIdAsync(userId);
            var followedPodcastIds = userId != null
                ? await _appUserPodcastService.GetFollowedPodcastIdsAsync(userId)
                : new List<int>();
            var followedPodcasts = await _appUserPodcastService.GetFollowedPodcastsAsync(userId);
            return View(new HomeVM
            {
                PodcastCategories = categories,
                Podcasts = podcasts,
                FollowedPodcastIds = followedPodcastIds,
                FollowedPodcasts = followedPodcasts,
                Playlists = playlists
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create-playlist")]
        public async Task<IActionResult> CreatePlaylist(string playlist_name)
        {
            if (string.IsNullOrWhiteSpace(playlist_name))
            {
                ModelState.AddModelError("", "Playlist ad? bo? ola bilm?z");
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
