using Service.ViewModels.Guest;
using Service.ViewModels.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IGuestService
    {
        Task CreateAsync(GuestCreateVM request);
        Task EditAsync(int id, GuestEditVM request);
        Task DeleteAsync(int id);
        Task<GuestAdminVM> GetByIdAsync(int id);
        Task<List<GuestAdminVM>> GetAllAsync();
    }
}
