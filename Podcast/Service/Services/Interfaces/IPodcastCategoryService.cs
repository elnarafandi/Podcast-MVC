using Service.ViewModels.PodcastCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IPodcastCategoryService
    {
        Task CreateAsync(PodcastCategoryCreateVM request);
        Task EditAsync(int id, PodcastCategoryEditVM request);
        Task DeleteAsync(int id);
        Task<PodcastCategoryAdminVM> GetByIdAsync(int id);
        Task<List<PodcastCategoryAdminVM>> GetAllAsync();
        Task<bool> ExistsByNameAsync(string name);
    }
}
