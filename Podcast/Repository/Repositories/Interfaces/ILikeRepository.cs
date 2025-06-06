using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface ILikeRepository:IBaseRepository<Like>
    {
        Task<bool> IsLikedAsync(string userId, int episodeId);
        Task<Like> GetAsync(string userId, int episodeId);
        Task<int> GetLikeCountAsync(int episodeId);
        Task<List<int>> GetLikedEpisodeIdsAsync(string userId, List<int> episodeIds);
    }
}
