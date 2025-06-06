using Domain.Entities;
using Service.ViewModels.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task CreatePlaylistAsync(Playlist playlist);
        Task<IEnumerable<Playlist>> GetPlaylistsByUserIdAsync(string userId);
        Task AddEpisodeToPlaylistAsync(int episodeId, int playlistId);
        Task<PlaylistAdminVM> GetByIdAsync(int id);
        Task<bool> IsEpisodeInPlaylistAsync(int episodeId, int playlistId);
        Task DeleteAsync(int id);
    }
}
