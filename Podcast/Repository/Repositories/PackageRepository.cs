using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class PackageRepository:BaseRepository<Package>,IPackageRepository
    {
        public PackageRepository(AppDbContext context) : base(context) { }

        public async Task<List<Package>> GetAllAsync()
        {
            return await _context.Set<Package>().ToListAsync();
        }

        public async Task<Package> GetByIdAsync(int id)
        {
            return await _context.Set<Package>().Include(p => p.AppUsers).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
