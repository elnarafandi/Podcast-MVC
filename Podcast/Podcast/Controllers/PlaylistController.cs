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
        public PlaylistController(IPlaylistService playlistService, 
                                 IPlaylistEpisodeService playlistEpisodeService,
                                 ILikeService likeService)
        {
            _playlistService = playlistService;
            _playlistEpisodeService = playlistEpisodeService;
            _likeService = likeService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var playlist= await _playlistService.GetByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<int> likedEpisodeIds = new();
            if (!string.IsNullOrEmpty(userId))
            {
                var episodeIds = playlist.PlaylistEpisodes.Select(pe => pe.Episode.Id).ToList();
                likedEpisodeIds = await _likeService.GetLikedEpisodeIdsAsync(userId, episodeIds);
            }
            return View(new PlaylistVM
            {
                Playlist = playlist,
                LikedEpisodeIds = likedEpisodeIds
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
