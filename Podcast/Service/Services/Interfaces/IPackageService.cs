using Domain.Entities;
using Service.ViewModels.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IPackageService
    {
        Task<PackageAdminVM> GetByIdAsync(int id);
        Task EditAsync(int id, PackageEditVM request);
        Task<List<PackageAdminVM>> GetAllAsync();
    }
}
