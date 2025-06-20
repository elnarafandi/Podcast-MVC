using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.Extensions;
using Service.Services.Interfaces;
using Service.ViewModels.PodcastCategory;
using Service.ViewModels.TeamMember;

namespace Podcast.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TeamMemberController : Controller
    {
        private readonly ITeamMemberService _teamMemberService;
        public TeamMemberController(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var teamMembers=await _teamMemberService.GetAllAsync();
            return View(teamMembers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMemberCreateVM request)
        {
            if (request.UploadImage == null)
            {
                ModelState.AddModelError("UploadImage", "Please upload an image.");
            }
            else
            {
                if (!request.UploadImage.CheckFileType("image"))
                {
                    ModelState.AddModelError("UploadImage", "Only image files are allowed.");
                }

                if (!request.UploadImage.CheckFileSize(1024)) // 1 MB
                {
                    ModelState.AddModelError("UploadImage", "Image size must be less than 1 MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _teamMemberService.CreateAsync(request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var teamMember= await _teamMemberService.GetByIdAsync(id);
            return View(new TeamMemberEditVM { FirstName=teamMember.FirstName, 
                                               LastName=teamMember.LastName,
                                               Email=teamMember.Email,
                                               Image=teamMember.Image,
                                               Information=teamMember.Information,
                                               SocialMedia=teamMember.SocialMedia});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamMemberEditVM request)
        {
            var existingMember = await _teamMemberService.GetByIdAsync(id);
            request.Image = existingMember.Image;

            if (request.UploadImage != null)
            {
                if (!request.UploadImage.CheckFileType("image"))
                {
                    ModelState.AddModelError("UploadImage", "Only image files are allowed.");
                }

                if (!request.UploadImage.CheckFileSize(1024)) // 1 MB
                {
                    ModelState.AddModelError("UploadImage", "Image size must be less than 1 MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _teamMemberService.EditAsync(id, request);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _teamMemberService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var teamMember = await _teamMemberService.GetByIdAsync(id);
            return View(teamMember);
        }
    }
}
