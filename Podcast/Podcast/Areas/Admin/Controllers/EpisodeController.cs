using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Helpers.Extensions;
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
            var podcasts= await _podcastService.GetAllAsync();
            return View(new EpisodeVM()
            {
                Episodes = episode,
                Podcasts = podcasts
            });
        }

        [HttpGet]
        public async Task<IActionResult> FilterByPodcast(int? podcastId)
        {
            if (podcastId == 0)
            {
                return BadRequest("PodcastId is required.");
            }

            List<EpisodeAdminVM> episodes;

            if (podcastId == null)
            {
                episodes= await _episodeService.GetAllAsync();
            }
            else
            {
                episodes = await _episodeService.GetEpisodesByPodcastIdAsync(podcastId);
            }
            
            
            var podcasts = await _podcastService.GetAllAsync();

            return View("Index", new EpisodeVM
            {
                Episodes = episodes,
                Podcasts = podcasts
            });
        }




        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var podcastsDb = await _podcastService.GetAllAsync();
            var podcasts = podcastsDb.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Title
            });

            var guestsDb = await _guestService.GetAllAsync();
            var guests = guestsDb.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Email
            });

            ViewBag.Podcasts = podcasts;
            ViewBag.Guests = guests;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EpisodeCreateVM request)
        {
            var podcastsDb = await _podcastService.GetAllAsync();
            ViewBag.Podcasts = podcastsDb.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Title
            });

            var guestsDb = await _guestService.GetAllAsync();
            ViewBag.Guests = guestsDb.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Email
            });

            // Əlavə ModelState yoxlamaları
            if (string.IsNullOrWhiteSpace(request.Title))
                ModelState.AddModelError("Title", "Title is required.");

            if (string.IsNullOrWhiteSpace(request.Description))
                ModelState.AddModelError("Description", "Description is required.");

            if (request.PodcastId <= 0)
                ModelState.AddModelError("PodcastId", "Please select a podcast.");

            if (request.UploadImage == null)
            {
                ModelState.AddModelError("UploadImage", "Please upload an image.");
            }
            else
            {
                if (!request.UploadImage.CheckFileType("image"))
                    ModelState.AddModelError("UploadImage", "Only image files are allowed.");
                if (!request.UploadImage.CheckFileSize(1 * 1024 * 1024))
                    ModelState.AddModelError("UploadImage", "Image file size must be less than 1 MB.");
            }

            if (request.AudioFile == null)
            {
                ModelState.AddModelError("AudioFile", "Please upload an audio file.");
            }
            else
            {
                if (!request.AudioFile.CheckFileType("audio"))
                    ModelState.AddModelError("AudioFile", "Only audio files are allowed.");
                if (!request.AudioFile.CheckFileSize(50 * 1024 * 1024))
                    ModelState.AddModelError("AudioFile", "Audio file size must be less than 50 MB.");
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _episodeService.CreateAsync(request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Title", ex.Message);
                return View(request);
            }

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
                Text = a.Email
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
                Text = a.Email
            });

            ViewBag.Podcasts = podcasts;
            ViewBag.Guests = guests;

            // Mövcud şəkil və audio itməsin deyə saxlayırıq
            var existingEpisode = await _episodeService.GetByIdAsync(id);
            request.Image = existingEpisode.Image;
            request.Audio = existingEpisode.Audio;

            // Şəkil yoxlamaları (isteğe bağlı, yüklənibsə yoxlanacaq)
            if (request.UploadImage != null)
            {
                if (!request.UploadImage.CheckFileType("image"))
                {
                    ModelState.AddModelError("UploadImage", "Only image files are allowed.");
                }
                if (!request.UploadImage.CheckFileSize(1024)) // 1 MB limit (1024 KB)
                {
                    ModelState.AddModelError("UploadImage", "Image size should be less than 1 MB.");
                }
            }

            // Audio yoxlamaları (isteğe bağlı)
            if (request.AudioFile != null)
            {
                if (!request.AudioFile.CheckFileType("audio"))
                {
                    ModelState.AddModelError("AudioFile", "Only audio files are allowed.");
                }
                if (!request.AudioFile.CheckFileSize(50 * 1024 * 1024)) // 50 MB limit
                {
                    ModelState.AddModelError("AudioFile", "Audio size should be less than 50 MB.");
                }
            }

            // Əsas sahələrin boş olmamasını da yoxlamaq yaxşıdır
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                ModelState.AddModelError("Title", "Title can't be empty.");
            }
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                ModelState.AddModelError("Description", "Description can't be empty.");
            }
            if (request.PodcastId <= 0)
            {
                ModelState.AddModelError("PodcastId", "Please select a valid podcast.");
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _episodeService.EditAsync(id, request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Title", ex.Message);
                return View(request);
            }

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
