using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IEpisodeRepository:IBaseRepository<Episode>
    {
        Task<Episode> GetByIdAsync(int id);
        Task<List<Episode>> GetAllAsync(int? count = null);
    }
}
