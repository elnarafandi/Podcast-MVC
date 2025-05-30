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
        Task<TeamMemberAdminVM> GetByIdAsync(int id);
        Task<List<TeamMemberAdminVM>> GetAllAsync();
    }
}
