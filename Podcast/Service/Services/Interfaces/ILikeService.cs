using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface ILikeService
    {
        Task ToggleLikeAsync(string userId, int episodeId);
        Task<int> GetLikeCountAsync(int episodeId);
        Task<bool> IsLikedAsync(string userId, int episodeId);
        Task<List<int>> GetLikedEpisodeIdsAsync(string userId, List<int> episodeIds);
    }
}
