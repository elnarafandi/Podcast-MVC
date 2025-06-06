using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PlaylistService:IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IPlaylistEpisodeRepository _playlistEpisodeRepository;
        public PlaylistService(IPlaylistRepository playlistRepository,
                               IPlaylistEpisodeRepository playlistEpisodeRepository)
        {
            _playlistRepository = playlistRepository;
            _playlistEpisodeRepository = playlistEpisodeRepository;
        }

        public async Task AddEpisodeToPlaylistAsync(int episodeId, int playlistId)
        {
            bool isInPlaylist = await _playlistRepository.IsEpisodeInPlaylistAsync(episodeId, playlistId);

            if (isInPlaylist)
            {
                throw new Exception("This episode is already in the playlist.");
            }

            await _playlistRepository.AddEpisodeToPlaylistAsync(episodeId, playlistId);
        }

        public async Task CreatePlaylistAsync(Playlist playlist)
        {
            await _playlistRepository.CreateAsync(playlist);
        }

        public async Task DeleteAsync(int id)
        {
            var playlist= await _playlistRepository.GetByIdAsync(id);
            foreach(var epidsode in playlist.PlaylistEpisodes)
            {
                await _playlistEpisodeRepository.DeleteAsync(epidsode);
            }
            await _playlistRepository.DeleteAsync(playlist);
        }

        public async Task<PlaylistAdminVM> GetByIdAsync(int id)
        {
            var playlistDb=await _playlistRepository.GetByIdAsync(id);
            PlaylistAdminVM playlist = new PlaylistAdminVM()
            {
                Id = playlistDb.Id,
                Name = playlistDb.Name,
                PlaylistEpisodes = playlistDb.PlaylistEpisodes
            };
            return playlist;
        }

        public async Task<IEnumerable<Playlist>> GetPlaylistsByUserIdAsync(string userId)
        {
            return await _playlistRepository.GetPlaylistsByUserIdAsync(userId);
        }

        public async Task<bool> IsEpisodeInPlaylistAsync(int episodeId, int playlistId)
        {
            return await _playlistRepository.IsEpisodeInPlaylistAsync(episodeId, playlistId);
        }
    }
}
