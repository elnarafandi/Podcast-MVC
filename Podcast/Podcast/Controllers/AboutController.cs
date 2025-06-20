using Domain.Entities;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        public AboutController(ITeamMemberService teamMemberService,
                               IEpisodeService episodeService,
                               ILogger<AboutController> logger,
                               UserManager<AppUser> userManager)
        {
            _teamMemberService = teamMemberService;
            _episodeService = episodeService;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            var allUsersDb = _userManager.Users.Where(u => u.PackageId == 3).ToList();

            foreach (var userDb in allUsersDb)
            {
                var adjustedTime = DateTime.UtcNow.AddHours(4);


                var daysSincePurchased = (adjustedTime - userDb.PurchasedAt).Days;

                if (daysSincePurchased >= 30)
                {
                    userDb.PackageId = 4;
                    userDb.PurchasedAt = adjustedTime;
                    await _userManager.UpdateAsync(userDb);
                    
                }
            }



            var teamMembers= await _teamMemberService.GetAllAsync();
            var episodes= await _episodeService.GetAllAsync(3);

            _logger.LogInformation($"A request has been received for the About Index.");

            return View(new AboutVM
            {
                TeamMembers = teamMembers,
                Episodes = episodes
            });
        }
    }
}
