using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class PodcastRepository:BaseRepository<Podcast>, IPodcastRepository
    {
        public PodcastRepository(AppDbContext context) : base(context) { }

        public async Task<List<Podcast>> GetAllAsync()
        {
            return await _context.Set<Podcast>()
    .Include(c => c.PodcastCategory)
    .Include(c => c.TeamMember)
    .Include(c => c.Episodes)
        .ThenInclude(e => e.Likes)               
    .Include(c => c.Episodes)
        .ThenInclude(e => e.EpisodeGuests)     
            .ThenInclude(eg => eg.Guest)        
    .OrderByDescending(c => c.CreatedDate)
    .ToListAsync();
        }

        public async Task<IEnumerable<Podcast>> GetAllByCategorySortedByFollowCountAsync(int categoryId)
        {
            return await _context.Podcasts
                .Where(p => p.PodcastCategoryId == categoryId)
                .OrderByDescending(p => _context.AppUserPodcasts.Count(ap => ap.PodcastId == p.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<Podcast>> GetAllWithConditionAsync(Expression<Func<Podcast, bool>> predicate)
        {
            return await _context.Set<Podcast>().Include(c => c.PodcastCategory).Include(c => c.TeamMember).Where(predicate).OrderByDescending(c=>c.CreatedDate).ToListAsync();
        }

        

        public async Task<Podcast> GetByIdAsync(int id)
        {
            return await _context.Set<Podcast>().Include(c => c.PodcastCategory).Include(c => c.TeamMember).Include(c => c.Episodes).ThenInclude(e => e.Likes).Include(c => c.Episodes).ThenInclude(e => e.EpisodeGuests).ThenInclude(eg => eg.Guest).FirstOrDefaultAsync(c => c.Id == id);
        }



        public async Task<IEnumerable<Podcast>> GetAllByCategorySortedByFollowCountShowMoreAsync(int categoryId, int skip = 0, int take = 8)
        {
            return await _context.Set<Podcast>()
    .Include(c => c.PodcastCategory)
    .Include(c => c.TeamMember)
    .Include(c => c.Episodes)
        .ThenInclude(e => e.Likes)
    .Include(c => c.Episodes)
        .ThenInclude(e => e.EpisodeGuests)
            .ThenInclude(eg => eg.Guest)
                .Where(p => p.PodcastCategoryId == categoryId)
                .OrderByDescending(p => _context.AppUserPodcasts.Count(ap => ap.PodcastId == p.Id))
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Podcast>> GetPodcastsAsync(int categoryId, int skip = 0, int take = 8)
        {
            return await _context.Set<Podcast>()
    .Include(c => c.PodcastCategory)
    .Include(c => c.TeamMember)
    .Include(c => c.Episodes)
        .ThenInclude(e => e.Likes)
    .Include(c => c.Episodes)
        .ThenInclude(e => e.EpisodeGuests)
            .ThenInclude(eg => eg.Guest)
                .Where(p => p.PodcastCategoryId == categoryId)
                .OrderByDescending(p => p.CreatedDate) // Default sort, sonuncu əlavə olunanlar üst sırada
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

    }
}
