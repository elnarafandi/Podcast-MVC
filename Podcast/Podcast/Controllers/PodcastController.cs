using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;
using Service.ViewModels.Podcast;
using System.Security.Claims;

namespace Podcast.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class PodcastController : Controller
    {
        private readonly IPodcastService _podcastService;
        private readonly IPodcastCategoryService _podcastCategoryService;
        private readonly IAppUserPodcastService _appUserPodcastService;
        private readonly ICommentService _commentService;
        private readonly IPlaylistService _playlistService;
        private readonly ILikeService _likeService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly UserManager<AppUser> _userManager;
        public PodcastController(IPodcastService podcastService, 
                                 IPodcastCategoryService podcastCategoryService,
                                 IAppUserPodcastService appUserPodcastService,
                                 ICommentService commentService,
                                 IPlaylistService playlistService,
                                 ILikeService likeService,
                                 UserManager<AppUser> userManager,
                                 ITeamMemberService teamMemberService)
        {
            _podcastService = podcastService;
            _podcastCategoryService = podcastCategoryService;
            _appUserPodcastService = appUserPodcastService;
            _commentService = commentService;
            _playlistService = playlistService;
            _likeService = likeService;
            _userManager = userManager;
            _teamMemberService = teamMemberService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            var allUsersDb = _userManager.Users.Where(u => u.PackageId == 3).ToList();

            foreach (var userDb in allUsersDb)
            {
                var adjustedTime = DateTime.UtcNow.AddHours(4);


                var daysSincePurchased = (adjustedTime - userDb.PurchasedAt).Days;

                if (daysSincePurchased >= 30)
                {
                    userDb.PackageId = 4;
                    userDb.PurchasedAt = adjustedTime;
                    await _userManager.UpdateAsync(userDb);
                    
                }
            }




            var podcast= await _podcastService.GetByIdAsync(id);
            if (podcast == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);


            bool isFollowing = false;
            if (!string.IsNullOrEmpty(userId))
            {
                isFollowing = await _appUserPodcastService.IsFollowingAsync(userId, id);
            }

            var comments= await _commentService.GetCommentsByPodcastIdAsync(podcast.Id);
            var playlists= await _playlistService.GetPlaylistsByUserIdAsync(userId);
            List<int> likedEpisodeIds = new();
            if (!string.IsNullOrEmpty(userId))
            {
                var episodeIds = podcast.Episodes.Select(e => e.Id).ToList();
                likedEpisodeIds = await _likeService.GetLikedEpisodeIdsAsync(userId, episodeIds);
            }
            return View(new PodcastDetailVM
            {
                Podcast = podcast,
                IsFollowing = isFollowing,
                Comments=comments,
                Playlists=playlists,
                LikedEpisodeIds = likedEpisodeIds,
                UserId = userId,
                User=user
            });
        }
        [HttpGet]
        public async Task<IActionResult> Search(string podcastTitle)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var podcasts= await _podcastService.SearchByTitleAsync(podcastTitle);
            var teamMembers= await _teamMemberService.GetAllAsync();
            var podcastCategories= await _podcastCategoryService.GetAllAsync();
            var followedPodcastIds = userId != null
                ? await _appUserPodcastService.GetFollowedPodcastIdsAsync(userId)
                : new List<int>();

            return View(new PodcastVM
            {
                Podcasts = podcasts,
                PodcastCategories = podcastCategories,
                SearchText= podcastTitle,
                FollowedPodcastIds= followedPodcastIds,
                TeamMembers= teamMembers
            });
        }
        [HttpGet]
        public async Task<IActionResult> Filter(string searchText, List<int>? categoryIds, List<int>? teamMemberIds)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var podcasts = await _podcastService.FilterAsync(searchText, categoryIds, teamMemberIds);
            var podcastCategories = await _podcastCategoryService.GetAllAsync();
            var teamMembers = await _teamMemberService.GetAllAsync();
            var followedPodcastIds = userId != null
                ? await _appUserPodcastService.GetFollowedPodcastIdsAsync(userId)
                : new List<int>();

            return View("Search", new PodcastVM
            {
                Podcasts = podcasts,
                PodcastCategories = podcastCategories,
                SearchText = searchText,
                FollowedPodcastIds = followedPodcastIds,
                TeamMembers = teamMembers,
                SelectedCategoryIds = categoryIds, // Selected categories
                SelectedTeamMemberIds = teamMemberIds // Selected team members
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFollow(int podcastId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            bool isFollowing = await _appUserPodcastService.IsFollowingAsync(userId, podcastId);

            if (isFollowing)
                await _appUserPodcastService.UnfollowAsync(userId, podcastId);
            else
                await _appUserPodcastService.FollowAsync(userId, podcastId);

            return RedirectToAction("Detail", new { id = podcastId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int podcastId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return RedirectToAction("Details", "Podcast", new { id = podcastId });
            }

            var comment = new Comment
            {
                PodcastId = podcastId,
                Content = content,
                AppUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            };

            await _commentService.AddCommentAsync(comment);

            return RedirectToAction("Detail", "Podcast", new { id = podcastId });
        }

        [HttpPost]
        public async Task<IActionResult> AddEpisodeToPlaylist(int episodeId, int playlistId, int podcastId)
        {
            if (playlistId == 0)
            {
                ModelState.AddModelError("", "Please select a playlist.");
                return RedirectToAction("Detail", new { id = podcastId });
            }

            try
            {
                bool isInPlaylist = await _playlistService.IsEpisodeInPlaylistAsync(episodeId, playlistId);
                if (isInPlaylist)
                {
                    TempData["PlaylistError"] = "This episode is already in the selected playlist.";
                    return RedirectToAction("Detail", new { id = podcastId });
                }

                await _playlistService.AddEpisodeToPlaylistAsync(episodeId, playlistId);
                return RedirectToAction("Detail", new { id = podcastId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Detail", new { id = podcastId });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ToggleEpisodeLike(int episodeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _likeService.ToggleLikeAsync(userId, episodeId);
            var likeCount = await _likeService.GetLikeCountAsync(episodeId);
            var isLiked = await _likeService.IsLikedAsync(userId, episodeId);

            return Json(new { likeCount, isLiked });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _commentService.GetByIdAsync(commentId);
            if (comment == null)
                return NotFound();

            // Yalnız istifadəçi şərhə sahibdirsə silə bilər
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.AppUserId != userId)
                return Forbid(); // İstifadəçinin şərhi silmək hüququ yoxdur.

            await _commentService.DeleteAsync(commentId);
            return RedirectToAction("Detail", new { id = comment.PodcastId });
        }
    }
}
