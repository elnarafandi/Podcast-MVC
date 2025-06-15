using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.About;

namespace Podcast.Controllers
{
    public class AboutController : Controller
    {
        private readonly ITeamMemberService _teamMemberService;
        private readonly IEpisodeService _episodeService;
        private readonly ILogger<AboutController> _logger;
        public AboutController(ITeamMemberService teamMemberService,
                               IEpisodeService episodeService,
                               ILogger<AboutController> logger)
        {
            _teamMemberService = teamMemberService;
            _episodeService = episodeService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            //_logger.LogInformation("Send request to GetAll method");
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
