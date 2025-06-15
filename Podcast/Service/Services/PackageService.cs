using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public async Task EditAsync(int id, PackageEditVM request)
        {
            var package= await _packageRepository.GetByIdAsync(id);
            
            package.Price = request.Price;
            await _packageRepository.EditAsync(package);
        }

        public async Task<List<PackageAdminVM>> GetAllAsync()
        {
            var packagesDb= await _packageRepository.GetAllAsync();
            var packages=packagesDb.Select(p=>new PackageAdminVM()
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();
            return packages;
        }

        public async Task<PackageAdminVM> GetByIdAsync(int id)
        {
            var packageDb=await _packageRepository.GetByIdAsync(id);
            var package = new PackageAdminVM()
            {
                Id=packageDb.Id,
                Name = packageDb.Name,
                Price = packageDb.Price,
                AppUsers = packageDb.AppUsers
            };
            return package;
        }
    }
}
