using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.About;

namespace Podcast.Controllers
{
    public class AboutController : Controller
    {
        private readonly ITeamMemberService _teamMemberService;
        private readonly IEpisodeService _episodeService;
        public AboutController(ITeamMemberService teamMemberService,
                               IEpisodeService episodeService)
        {
            _teamMemberService = teamMemberService;
            _episodeService = episodeService;
        }
        public async Task<IActionResult> Index()
        {
            var teamMembers= await _teamMemberService.GetAllAsync();
            var episodes= await _episodeService.GetAllAsync(3);
            return View(new AboutVM
            {
                TeamMembers = teamMembers,
                Episodes = episodes
            });
        }
    }
}
