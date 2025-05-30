using Service.ViewModels.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface ITeamMemberService
    {
        Task CreateAsync(TeamMemberCreateVM request);
        Task EditAsync(int id,TeamMemberEditVM request);
        Task DeleteAsync(int id);
        Task<TeamMemberAdminVM> GetByIdAsync(int id);
        Task<List<TeamMemberAdminVM>> GetAllAsync();
    }
}
