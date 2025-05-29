using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Podcast.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        
    }
}
