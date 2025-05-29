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
    public class PodcastCategoryRepository:BaseRepository<PodcastCategory>,IPodcastCategoryRepository
    {
        public PodcastCategoryRepository(AppDbContext context) : base(context) { }

        public async Task<List<PodcastCategory>> GetAllAsync()
        {
            return await _context.Set<PodcastCategory>().Include(pc=>pc.Podcasts).ToListAsync();
        }

        public async Task<PodcastCategory> GetByIdAsync(int id)
        {
            return await _context.Set<PodcastCategory>().Include(pc=>pc.Podcasts).FirstOrDefaultAsync(gc => gc.Id == id);
        }
    }
}
