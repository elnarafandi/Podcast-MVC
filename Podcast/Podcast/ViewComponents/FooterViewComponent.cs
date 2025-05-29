using Microsoft.AspNetCore.Mvc;

namespace Podcast.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
