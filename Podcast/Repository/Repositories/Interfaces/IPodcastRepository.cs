using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IPodcastRepository:IBaseRepository<Podcast>
    {
        Task<Podcast> GetByIdAsync(int id);
        Task<List<Podcast>> GetAllAsync();
    }
}
