using Service.ViewModels.Podcast;
using Service.ViewModels.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IPodcastService
    {
        Task CreateAsync(PodcastCreateVM request);
        Task EditAsync(int id, PodcastEditVM request);
        Task DeleteAsync(int id);
        Task<PodcastAdminVM> GetByIdAsync(int id);
        Task<List<PodcastAdminVM>> GetAllAsync();
        Task<IEnumerable<PodcastAdminVM>> SearchByTitleAsync(string searchText);
        Task<IEnumerable<PodcastAdminVM>> FilterByCategoryAsync(string searchText,string categoryName);
        Task<IEnumerable<PodcastAdminVM>> GetAllByCategoryAsync(int categoryId);
        Task<IEnumerable<PodcastAdminVM>> GetPodcastsAsync(int skip, int take,int categoryId);
    }
}
