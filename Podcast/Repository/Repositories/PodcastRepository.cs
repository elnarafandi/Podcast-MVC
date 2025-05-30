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
    public class PodcastRepository:BaseRepository<Podcast>, IPodcastRepository
    {
        public PodcastRepository(AppDbContext context) : base(context) { }

        public async Task<List<Podcast>> GetAllAsync()
        {
            return await _context.Set<Podcast>().Include(c=>c.PodcastCategory).Include(c=>c.TeamMember).ToListAsync();
        }

        public async Task<Podcast> GetByIdAsync(int id)
        {
            return await _context.Set<Podcast>().Include(c => c.PodcastCategory).Include(c => c.TeamMember).FirstOrDefaultAsync(c=>c.Id==id);
        }
    }
}
