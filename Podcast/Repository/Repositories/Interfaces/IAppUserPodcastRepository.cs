using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IAppUserPodcastRepository:IBaseRepository<AppUserPodcast>
    {
        Task<AppUserPodcast> GetAsync(string userId, int podcastId);
        Task<List<int>> GetFollowedPodcastIdsByUserIdAsync(string userId);
        Task<List<Podcast>> GetFollowedPodcastsByUserIdAsync(string userId);
    }
}
