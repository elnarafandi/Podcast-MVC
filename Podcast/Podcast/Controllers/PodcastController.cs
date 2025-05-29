using Microsoft.AspNetCore.Mvc;

namespace Podcast.Controllers
{
    public class PodcastController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail()
        {
            return View();
        }
    }
}
