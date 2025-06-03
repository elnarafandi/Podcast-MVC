using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IPodcastCategoryRepository:IBaseRepository<PodcastCategory>
    {
        Task<PodcastCategory> GetByIdAsync(int id);
        Task<List<PodcastCategory>> GetAllAsync();
    }
}
