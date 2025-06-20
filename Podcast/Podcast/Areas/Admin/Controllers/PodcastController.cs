using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Helpers.Extensions;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Podcast;
using Service.ViewModels.TeamMember;

namespace Podcast.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PodcastController : Controller
    {
        private readonly IPodcastService _podcastService;
        private readonly IPodcastCategoryService _podcastCategoryService;
        private readonly ITeamMemberService _teamMemberService;
        public PodcastController(IPodcastService podcastService, 
                                 IPodcastCategoryService podcastCategoryService,
                                 ITeamMemberService teamMemberService)
        {
            _podcastService = podcastService;
            _podcastCategoryService = podcastCategoryService;
            _teamMemberService = teamMemberService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var podcasts= await _podcastService.GetAllAsync();
            return View(podcasts);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var teamMembersDb= await _teamMemberService.GetAllAsync();
            var teamMembers = teamMembersDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Email
            });
            var categoriesDb= await _podcastCategoryService.GetAllAsync();
            var categories = categoriesDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });

            ViewBag.TeamMembers = teamMembers;
            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PodcastCreateVM request)
        {
            var teamMembersDb = await _teamMemberService.GetAllAsync();
            var teamMembers = teamMembersDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Email
            });
            var categoriesDb = await _podcastCategoryService.GetAllAsync();
            var categories = categoriesDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });

            ViewBag.TeamMembers = teamMembers;
            ViewBag.Categories = categories;

            if (request.UploadImage == null)
            {
                ModelState.AddModelError("UploadImage", "Please upload an image file.");
            }
            else
            {
                if (!request.UploadImage.CheckFileType("image"))
                {
                    ModelState.AddModelError("UploadImage", "Only image files are allowed.");
                }

                if (!request.UploadImage.CheckFileSize(1024)) // 1MB limit
                {
                    ModelState.AddModelError("UploadImage", "File size should be less than 1 MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _podcastService.CreateAsync(request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Title", "A podcast with the same title already exists.");
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var teamMembersDb = await _teamMemberService.GetAllAsync();
            var teamMembers = teamMembersDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Email
            });
            var categoriesDb = await _podcastCategoryService.GetAllAsync();
            var categories = categoriesDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });

            ViewBag.TeamMembers = teamMembers;
            ViewBag.Categories = categories;

            var podcast = await _podcastService.GetByIdAsync(id);

            return View(new PodcastEditVM
            {
                Title = podcast.Title,
                Description = podcast.Description,
                Image = podcast.Image,
                TeamMemberId = podcast.TeamMember.Id,
                PodcastCategoryId = podcast.PodcastCategory.Id
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PodcastEditVM request)
        {
            var teamMembersDb = await _teamMemberService.GetAllAsync();
            var teamMembers = teamMembersDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Email
            });
            var categoriesDb = await _podcastCategoryService.GetAllAsync();
            var categories = categoriesDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });

            ViewBag.TeamMembers = teamMembers;
            ViewBag.Categories = categories;

            var existingPodcast = await _podcastService.GetByIdAsync(id);
            request.Image = existingPodcast.Image;

            if (request.UploadImage != null)
            {
                if (!request.UploadImage.CheckFileType("image"))
                {
                    ModelState.AddModelError("UploadImage", "Only image files are allowed.");
                }

                if (!request.UploadImage.CheckFileSize(1024)) // 1 MB
                {
                    ModelState.AddModelError("UploadImage", "File size should be less than 1 MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _podcastService.EditAsync(id, request);
            }
            catch (InvalidOperationException ex)
            {
                request.Image = existingPodcast.Image;
                ModelState.AddModelError("Title", ex.Message);
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _podcastService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var podcast = await _podcastService.GetByIdAsync(id);
            return View(podcast);
        }
    }
}
