using Domain.Entities;
using Service.ViewModels.Podcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IAppUserPodcastService
    {
        Task FollowAsync(string userId, int podcastId);
        Task UnfollowAsync(string userId, int podcastId);
        Task<bool> IsFollowingAsync(string userId, int podcastId);
        Task<List<int>> GetFollowedPodcastIdsAsync(string userId);
        Task<List<PodcastAdminVM>> GetFollowedPodcastsAsync(string userId);
    }
}
