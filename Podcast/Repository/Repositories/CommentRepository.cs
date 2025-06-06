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
    public class CommentRepository:BaseRepository<Comment>,ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Comment>> GetCommentsByPodcastIdAsync(int podcastId)
        {
            return await _context.Set<Comment>().Include(c => c.AppUser).Where(c => c.PodcastId == podcastId).ToListAsync();
        }
    }
}
