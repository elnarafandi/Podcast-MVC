using Service.ViewModels.Episode;
using Service.ViewModels.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IEpisodeService
    {
        Task CreateAsync(EpisodeCreateVM request);
        Task EditAsync(int id, EpisodeEditVM request);
        Task DeleteAsync(int id);
        Task<EpisodeAdminVM> GetByIdAsync(int id);
        Task<List<EpisodeAdminVM>> GetAllAsync(int? count = null);
    }
}
