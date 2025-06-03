using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                Text = a.SocialMedia
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
            if (!ModelState.IsValid) return View(request);
            await _podcastService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var teamMembersDb = await _teamMemberService.GetAllAsync();
            var teamMembers = teamMembersDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = $"{a.FirstName} {a.LastName}"
            });
            var categoriesDb = await _podcastCategoryService.GetAllAsync();
            var categories = categoriesDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });

            ViewBag.TeamMembers = teamMembers;
            ViewBag.Categories = categories;

            var podcast= await _podcastService.GetByIdAsync(id);

            return View(new PodcastEditVM { Title=podcast.Title,
                                            Description=podcast.Description,
                                            Image=podcast.Image,
                                            TeamMemberId=podcast.TeamMember.Id,
                                            PodcastCategoryId=podcast.PodcastCategory.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PodcastEditVM request)
        {
            var teamMembersDb = await _teamMemberService.GetAllAsync();
            var teamMembers = teamMembersDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = $"{a.FirstName} {a.LastName}"
            });
            var categoriesDb = await _podcastCategoryService.GetAllAsync();
            var categories = categoriesDb.Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });

            ViewBag.TeamMembers = teamMembers;
            ViewBag.Categories = categories;
            await _podcastService.EditAsync(id, request);
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
