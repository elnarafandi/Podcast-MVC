using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.PodcastCategory;
using Service.ViewModels.TeamMember;

namespace Podcast.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeamMemberController : Controller
    {
        private readonly ITeamMemberService _teamMemberService;
        public TeamMemberController(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var teamMembers=await _teamMemberService.GetAllAsync();
            return View(teamMembers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMemberCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);
            await _teamMemberService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var teamMember= await _teamMemberService.GetByIdAsync(id);
            return View(new TeamMemberEditVM { FirstName=teamMember.FirstName, 
                                               LastName=teamMember.LastName,
                                               Image=teamMember.Image,
                                               Information=teamMember.Information,
                                               SocialMedia=teamMember.SocialMedia});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamMemberEditVM request)
        {
            await _teamMemberService.EditAsync(id, request);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _teamMemberService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var teamMember = await _teamMemberService.GetByIdAsync(id);
            return View(teamMember);
        }
    }
}
