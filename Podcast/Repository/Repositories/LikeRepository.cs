using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class LikeRepository:BaseRepository<Like>,ILikeRepository
    {
        public LikeRepository(AppDbContext context) : base(context) { }

        public async Task<Like> GetAsync(string userId, int episodeId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.AppUserId == userId && l.EpisodeId == episodeId);
        }

        public async Task<int> GetLikeCountAsync(int episodeId)
        {
            return await _context.Likes.CountAsync(l => l.EpisodeId == episodeId);
        }

        public async Task<bool> IsLikedAsync(string userId, int episodeId)
        {
            return await _context.Likes.AnyAsync(l => l.AppUserId == userId && l.EpisodeId == episodeId);
        }
        public async Task<List<int>> GetLikedEpisodeIdsAsync(string userId, List<int> episodeIds)
        {
            return await _context.Likes
                .Where(l => l.AppUserId == userId && episodeIds.Contains(l.EpisodeId))
                .Select(l => l.EpisodeId)
                .ToListAsync();
        }
    }
}
