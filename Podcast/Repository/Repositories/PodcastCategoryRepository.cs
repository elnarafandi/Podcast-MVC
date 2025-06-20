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
    public class PodcastCategoryRepository:BaseRepository<PodcastCategory>,IPodcastCategoryRepository
    {
        public PodcastCategoryRepository(AppDbContext context) : base(context) { }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.PodcastCategories
                .AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<List<PodcastCategory>> GetAllAsync()
        {
            return await _context.Set<PodcastCategory>().Include(pc=>pc.Podcasts).OrderByDescending(pc=>pc.CreatedDate).ToListAsync();
        }

        

        public async Task<PodcastCategory> GetByIdAsync(int id)
        {
            return await _context.Set<PodcastCategory>().Include(pc=>pc.Podcasts).ThenInclude(p=>p.TeamMember).FirstOrDefaultAsync(gc => gc.Id == id);
        }
    }
}
