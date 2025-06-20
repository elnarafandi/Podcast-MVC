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


        public async Task<IEnumerable<Podcast>> GetAllByCategorySortedByFollowCountLessShowMoreAsync(int categoryId, int skip = 0, int take = 8)
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
                .OrderBy(p => _context.AppUserPodcasts.Count(ap => ap.PodcastId == p.Id))
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
                .OrderByDescending(p => p.CreatedDate) 
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Podcast>> FilterAsync(string searchText, List<int>? categoryIds, List<int>? teamMemberIds)
        {
            var query = _context.Set<Podcast>()
                                .Include(p => p.TeamMember)
                                .Include(p => p.PodcastCategory)
                                .Include(p => p.Episodes)
                                .Include(p => p.AppUserPodcasts)
                                .Include(p => p.Comments)
                                .AsQueryable();

            // Search text filter
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(p => p.Title.Contains(searchText));
            }

            // Category filter
            if (categoryIds != null && categoryIds.Any())
            {
                query = query.Where(p => categoryIds.Contains(p.PodcastCategoryId));
            }

            // Team member filter
            if (teamMemberIds != null && teamMemberIds.Any())
            {
                query = query.Where(p => teamMemberIds.Contains(p.TeamMemberId));
            }

            return await query.ToListAsync();
        }



        public async Task<IEnumerable<Podcast>> GetAllByCategorySortedByOldestAsync(int categoryId, int skip, int take)
        {
            return await _context.Set<Podcast>().Include(c => c.PodcastCategory)
    .Include(c => c.TeamMember)
    .Include(c => c.Episodes)
        .ThenInclude(e => e.Likes)
    .Include(c => c.Episodes)
        .ThenInclude(e => e.EpisodeGuests)
            .ThenInclude(eg => eg.Guest)
                                 .Where(p => p.PodcastCategoryId == categoryId)
                                 .OrderBy(p => p.CreatedDate) 
                                 .Skip(skip)
                                 .Take(take)
                                 .ToListAsync();
        }




    }
}
