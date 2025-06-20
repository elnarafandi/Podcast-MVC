using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IPlaylistRepository:IBaseRepository<Playlist>
    {
        Task<IEnumerable<Playlist>> GetPlaylistsByUserIdAsync(string userId);
        Task AddEpisodeToPlaylistAsync(int episodeId, int playlistId);
        Task<bool> IsEpisodeInPlaylistAsync(int episodeId, int playlistId);
        Task<Playlist> GetByIdAsync(int id);
        Task<bool> PlaylistExistsAsync(string name, string userId);
    }
}
