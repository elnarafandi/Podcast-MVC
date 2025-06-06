using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Playlist;
using System.Security.Claims;

namespace Podcast.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly IPlaylistEpisodeService _playlistEpisodeService;
        public PlaylistController(IPlaylistService playlistService, 
                                 IPlaylistEpisodeService playlistEpisodeService)
        {
            _playlistService = playlistService;
            _playlistEpisodeService = playlistEpisodeService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var playlist= await _playlistService.GetByIdAsync(id);
            return View(playlist);
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
    }
}
