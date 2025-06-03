using Service.ViewModels.Episode;
using Service.ViewModels.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.About
{
    public class AboutVM
    {
        public IEnumerable<TeamMemberAdminVM> TeamMembers { get; set; }
        public IEnumerable<EpisodeAdminVM> Episodes { get; set; }
    }
}
