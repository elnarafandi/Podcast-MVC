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
    public class AppUserPodcastRepository:BaseRepository<AppUserPodcast>,IAppUserPodcastRepository
    {
        public AppUserPodcastRepository(AppDbContext context) : base(context) { }

        public async Task<AppUserPodcast> GetAsync(string userId, int podcastId)
        {
            return await _context.Set<AppUserPodcast>().FirstOrDefaultAsync(x => x.AppUserId == userId && x.PodcastId == podcastId);
        }

        public async Task<List<int>> GetFollowedPodcastIdsByUserIdAsync(string userId)
        {
            return await _context.Set<AppUserPodcast>().Where(up => up.AppUserId == userId).Select(up => up.PodcastId).ToListAsync();
        }

        public async Task<List<Podcast>> GetFollowedPodcastsByUserIdAsync(string userId)
        {
            return await _context.Set<AppUserPodcast>().Where(x => x.AppUserId == userId).Include(x => x.Podcast).ThenInclude(p => p.TeamMember).Select(x => x.Podcast).ToListAsync();
        }
    }
}
