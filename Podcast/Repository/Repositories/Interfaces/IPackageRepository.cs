using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IPackageRepository:IBaseRepository<Package>
    {
        Task<Package> GetByIdAsync(int id);
        Task<List<Package>> GetAllAsync();
    }
}
