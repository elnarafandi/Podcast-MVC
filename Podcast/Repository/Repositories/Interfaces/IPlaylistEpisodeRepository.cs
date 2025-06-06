using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IPlaylistEpisodeRepository:IBaseRepository<PlaylistEpisode>
    {
        Task<PlaylistEpisode> GetPlaylistEpisodeAsync(int playlistId, int episodeId);
    }
}
