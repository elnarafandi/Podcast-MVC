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
    public class TeamMemberRepository:BaseRepository<TeamMember>,ITeamMemberRepository
    {
        public TeamMemberRepository(AppDbContext context) : base(context) { }

        public async Task<List<TeamMember>> GetAllAsync()
        {
            return await _context.Set<TeamMember>().Include(tm=>tm.Podcasts).OrderByDescending(c => c.CreatedDate).ToListAsync();
        }

        public async Task<TeamMember> GetByIdAsync(int id)
        {
            return await _context.Set<TeamMember>().Include(tm => tm.Podcasts).FirstOrDefaultAsync(tm => tm.Id == id);
        }
    }
}
