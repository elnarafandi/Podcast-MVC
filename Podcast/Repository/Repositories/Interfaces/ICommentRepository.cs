using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface ICommentRepository:IBaseRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByPodcastIdAsync(int podcastId);
        Task<Comment> GetByIdAsync(int id);
        Task<IEnumerable<Comment>> GetAllAsync();
    }
}
