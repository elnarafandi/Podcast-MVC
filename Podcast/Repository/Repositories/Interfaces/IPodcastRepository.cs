using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IPodcastRepository:IBaseRepository<Podcast>
    {
        Task<Podcast> GetByIdAsync(int id);
        Task<List<Podcast>> GetAllAsync();
        Task<IEnumerable<Podcast>> GetAllWithConditionAsync(Expression<Func<Podcast, bool>> predicate);
        Task<IEnumerable<Podcast>> GetPodcastsAsync(int skip, int take,int categoryId);
    }
}
