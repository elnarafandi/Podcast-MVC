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
        Task<IEnumerable<Podcast>> GetAllByCategorySortedByFollowCountAsync(int categoryId);

        Task<IEnumerable<Podcast>> GetAllByCategorySortedByFollowCountShowMoreAsync(int categoryId, int skip = 0, int take = 8);
        Task<IEnumerable<Podcast>>  GetPodcastsAsync(int categoryId, int skip = 0, int take = 8);
    }
}
