using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;

namespace Podcast.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class GuestController : Controller
    {
        private readonly IGuestService _guestService;
        private readonly ILogger<GuestController> _logger;
        public GuestController(IGuestService guestService, 
                               ILogger<GuestController> logger)
        {
            _guestService = guestService;
            _logger = logger;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var guest= await _guestService.GetByIdAsync(id);
            _logger.LogInformation($"A request has been received for the Guest Detail.");
            return View(guest);
        }
    }
}
