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
        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var guest= await _guestService.GetByIdAsync(id);
            return View(guest);
        }
    }
}
