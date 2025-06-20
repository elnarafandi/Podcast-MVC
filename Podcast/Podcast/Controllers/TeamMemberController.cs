using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace Podcast.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class TeamMemberController : Controller
    {
        private readonly ITeamMemberService _teamMemberService;
        private readonly ILogger<TeamMemberController> _logger;
        public TeamMemberController(ITeamMemberService teamMemberService, 
                                    ILogger<TeamMemberController> logger)
        {
            _teamMemberService = teamMemberService;
            _logger = logger;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var teamMember= await _teamMemberService.GetByIdAsync(id);
            _logger.LogInformation($"A request has been received for the TeamMember Detail.");
            return View(teamMember);
        }
    }
}
