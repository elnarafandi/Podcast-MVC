using Domain.Entities;
using Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface ITeamMemberRepository:IBaseRepository<TeamMember>
    {
        Task<TeamMember> GetByIdAsync(int id);
        Task<List<TeamMember>> GetAllAsync();
    }
}
