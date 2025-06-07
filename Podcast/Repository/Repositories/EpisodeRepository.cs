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
    public class EpisodeRepository:BaseRepository<Episode>,IEpisodeRepository
    {
        public EpisodeRepository(AppDbContext context) : base(context) { }

        

        public async Task<List<Episode>> GetAllAsync(int? count = null)
        {
            var episodes= await _context.Set<Episode>().Include(e=>e.Podcast).Include(e=>e.Likes).Include(e=>e.EpisodeGuests).ThenInclude(eg=>eg.Guest).OrderByDescending(c => c.CreatedDate).ToListAsync();
            if (count.HasValue)
            {
                episodes=episodes.Take(count.Value).ToList();
            }
            return episodes;
        }

        public async Task<Episode> GetByIdAsync(int id)
        {
            return await _context.Set<Episode>().Include(e => e.Podcast).Include(e => e.Likes).Include(e => e.EpisodeGuests).ThenInclude(eg => eg.Guest).FirstOrDefaultAsync(g=>g.Id==id);
        }

        
    }
}
