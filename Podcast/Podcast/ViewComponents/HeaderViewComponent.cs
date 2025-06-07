using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.ViewModels.Header;

namespace Podcast.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(new HeaderVM
            {
                Image=user?.Image
            });
        }
    }
}
