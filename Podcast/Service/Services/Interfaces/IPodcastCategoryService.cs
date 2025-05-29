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
        Task CreateAsync(PodcastCategoryCreateVM podcastCategory);
        Task EditAsync(int id, PodcastCategoryEditVM podcastCategory);
        Task DeleteAsync(int id);
        Task<PodcastCategoryAdminVM> GetByIdAsync(int id);
        Task<List<PodcastCategoryAdminVM>> GetAllAsync();
    }
}
