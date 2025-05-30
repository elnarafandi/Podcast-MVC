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
    public class GuestRepository:BaseRepository<Guest>,IGuestRepository
    {
        public GuestRepository(AppDbContext context) : base(context) { }

        public async Task<List<Guest>> GetAllAsync()
        {
            return await _context.Set<Guest>().Include(eg => eg.EpisodeGuests).ThenInclude(g=>g.Episode).ToListAsync();
        }

        public async Task<Guest> GetByIdAsync(int id)
        {
            return await _context.Set<Guest>().Include(eg => eg.EpisodeGuests).ThenInclude(g => g.Episode).FirstOrDefaultAsync(tm => tm.Id == id);
        }
    }
}
