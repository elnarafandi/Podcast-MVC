using Microsoft.AspNetCore.Mvc;

namespace Podcast.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
