using Microsoft.AspNetCore.Mvc;

namespace Podcast.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
