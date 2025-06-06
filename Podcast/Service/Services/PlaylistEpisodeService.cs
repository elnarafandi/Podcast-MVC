using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PlaylistEpisodeService:IPlaylistEpisodeService
    {
        private readonly IPlaylistEpisodeRepository _playlistEpisodeRepository;
        public PlaylistEpisodeService(IPlaylistEpisodeRepository playlistEpisodeRepository)
        {
            _playlistEpisodeRepository = playlistEpisodeRepository;
        }

        public async Task<bool> DeleteEpisodeAsync(int playlistId, int episodeId)
        {
            var playlistEpisode = await _playlistEpisodeRepository.GetPlaylistEpisodeAsync(playlistId, episodeId);
            if (playlistEpisode != null)
            {
                await _playlistEpisodeRepository.DeleteAsync(playlistEpisode);
                return true;
            }
            return false;
        }
    }
}
