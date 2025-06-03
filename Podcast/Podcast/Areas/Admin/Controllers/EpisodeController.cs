using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Services.Interfaces;
using Service.ViewModels.Episode;
using Service.ViewModels.Podcast;

namespace Podcast.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EpisodeController : Controller
    {
        private readonly IEpisodeService _episodeService;
        private readonly IPodcastService _podcastService;
        private readonly IGuestService _guestService;
        public EpisodeController(IEpisodeService episodeService, 
                                 IPodcastService podcastService,
                                 IGuestService guestService)
        {
            _episodeService = episodeService;
            _podcastService = podcastService;
            _guestService = guestService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var episode = await _episodeService.GetAllAsync();
            return View(episode);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var podcastsDb= await _podcastService.GetAllAsync();
            var podcasts = podcastsDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Title
            });
            var guestsDb=await _guestService.GetAllAsync();
            var guests = guestsDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.SocialMedia
            });

            ViewBag.Podcasts = podcasts;
            ViewBag.Guests = guests;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EpisodeCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);
            await _episodeService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var podcastsDb = await _podcastService.GetAllAsync();
            var podcasts = podcastsDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Title
            });
            var guestsDb = await _guestService.GetAllAsync();
            var guests = guestsDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.SocialMedia
            });

            ViewBag.Podcasts = podcasts;
            ViewBag.Guests = guests;

            var episode=await _episodeService.GetByIdAsync(id);
            return View(new EpisodeEditVM
            {
                Title = episode.Title,
                Description = episode.Description,
                Image= episode.Image,
                Audio= episode.Audio,
                PodcastId = episode.Podcast.Id,
                GuestIds = episode.EpisodeGuests.Select(pd => pd.GuestId).ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EpisodeEditVM request)
        {
            var podcastsDb = await _podcastService.GetAllAsync();
            var podcasts = podcastsDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Title
            });
            var guestsDb = await _guestService.GetAllAsync();
            var guests = guestsDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.SocialMedia
            });

            ViewBag.Podcasts = podcasts;
            ViewBag.Guests = guests;

            await _episodeService.EditAsync(id, request);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _episodeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var episode= await _episodeService.GetByIdAsync(id);
            return View(episode);
        }

    }
}
