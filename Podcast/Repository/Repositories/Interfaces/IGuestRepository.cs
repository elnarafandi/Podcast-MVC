using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IGuestRepository:IBaseRepository<Guest>
    {
        Task<Guest> GetByIdAsync(int id);
        Task<List<Guest>> GetAllAsync();
    }
}
