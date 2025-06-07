using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace Podcast.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class TeamMemberController : Controller
    {
        private readonly ITeamMemberService _teamMemberService;
        public TeamMemberController(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var teamMember= await _teamMemberService.GetByIdAsync(id);
            return View(teamMember);
        }
    }
}
