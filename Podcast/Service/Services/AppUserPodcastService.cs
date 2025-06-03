using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Podcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AppUserPodcastService : IAppUserPodcastService
    {
        private readonly IAppUserPodcastRepository _appUserPodcastRepository;
        public AppUserPodcastService(IAppUserPodcastRepository appUserPodcastRepository)
        {
            _appUserPodcastRepository = appUserPodcastRepository;
        }
        public async Task FollowAsync(string userId, int podcastId)
        {
            var existing = await _appUserPodcastRepository.GetAsync(userId, podcastId);
            if (existing == null)
            {
                var follow = new AppUserPodcast
                {
                    AppUserId = userId,
                    PodcastId = podcastId
                };
                await _appUserPodcastRepository.CreateAsync(follow);
            }
        }

        public async Task UnfollowAsync(string userId, int podcastId)
        {
            var existing = await _appUserPodcastRepository.GetAsync(userId, podcastId);
            if (existing != null)
            {
                await _appUserPodcastRepository.DeleteAsync(existing);
            }
        }

        public async Task<bool> IsFollowingAsync(string userId, int podcastId)
        {
            var existing = await _appUserPodcastRepository.GetAsync(userId, podcastId);
            return existing != null;
        }

        public async Task<List<int>> GetFollowedPodcastIdsAsync(string userId)
        {
            return await _appUserPodcastRepository.GetFollowedPodcastIdsByUserIdAsync(userId);
        }

        public async Task<List<PodcastAdminVM>> GetFollowedPodcastsAsync(string userId)
        {
            var podcastsDb= await _appUserPodcastRepository.GetFollowedPodcastsByUserIdAsync(userId);
            var podcasts = podcastsDb.Select(m => new PodcastAdminVM
            {
                Id = m.Id,
                Title = m.Title,
                Image = m.Image
            }).ToList();
            return podcasts;
        }
    }
}
