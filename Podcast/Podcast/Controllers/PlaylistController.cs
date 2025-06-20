using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Playlist;
using System.Security.Claims;

namespace Podcast.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly IPlaylistEpisodeService _playlistEpisodeService;
        private readonly ILikeService _likeService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<PlaylistController> _logger;
        public PlaylistController(IPlaylistService playlistService, 
                                 IPlaylistEpisodeService playlistEpisodeService,
                                 ILikeService likeService,
                                 UserManager<AppUser> userManager,
                                 ILogger<PlaylistController> logger)
        {
            _playlistService = playlistService;
            _playlistEpisodeService = playlistEpisodeService;
            _likeService = likeService;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var playlist= await _playlistService.GetByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            List<int> likedEpisodeIds = new();
            if (!string.IsNullOrEmpty(userId))
            {
                var episodeIds = playlist.PlaylistEpisodes.Select(pe => pe.Episode.Id).ToList();
                likedEpisodeIds = await _likeService.GetLikedEpisodeIdsAsync(userId, episodeIds);
            }

            _logger.LogInformation($"A request has been received for the Playlist Detail.");

            return View(new PlaylistVM
            {
                Playlist = playlist,
                LikedEpisodeIds = likedEpisodeIds,
                User=user
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteEpisode(int playlistId, int episodeId)
        {
            var result = await _playlistEpisodeService.DeleteEpisodeAsync(playlistId, episodeId);
            if (result)
            {
                return RedirectToAction("Detail", new { id = playlistId });
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            await _playlistService.DeleteAsync(id);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> ToggleEpisodeLike(int episodeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _likeService.ToggleLikeAsync(userId, episodeId);
            var likeCount = await _likeService.GetLikeCountAsync(episodeId);
            var isLiked = await _likeService.IsLikedAsync(userId, episodeId);

            return Json(new { likeCount, isLiked });
        }
    }
}
