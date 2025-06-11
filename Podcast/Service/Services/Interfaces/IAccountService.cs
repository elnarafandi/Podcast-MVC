using Service.ViewModels.Account;
using Service.ViewModels.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IAccountService
    {
        Task CreateRolesAsync();
        Task RegisterAsync(RegisterVM model);
        Task<bool> LoginAsync(LoginVM model);
        Task LogoutAsync();
        Task EditAccountAsync(string id, AccountEditVM request);
    }
}
