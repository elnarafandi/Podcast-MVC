using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class LikeService:ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }
        public async Task ToggleLikeAsync(string userId, int episodeId)
        {
            var isLiked = await _likeRepository.IsLikedAsync(userId, episodeId);
            if (isLiked)
            {
                var like = await _likeRepository.GetAsync(userId, episodeId);
                await _likeRepository.DeleteAsync(like);
            }
            else
            {
                var like = new Like
                {
                    AppUserId = userId,
                    EpisodeId = episodeId
                };
                await _likeRepository.CreateAsync(like);
            }
        }

        public Task<int> GetLikeCountAsync(int episodeId)
        {
            return _likeRepository.GetLikeCountAsync(episodeId);
        }

        public Task<bool> IsLikedAsync(string userId, int episodeId)
        {
            return _likeRepository.IsLikedAsync(userId, episodeId);
        }
        public async Task<List<int>> GetLikedEpisodeIdsAsync(string userId, List<int> episodeIds)
        {
            return await _likeRepository.GetLikedEpisodeIdsAsync(userId, episodeIds);
        }
    }
}
