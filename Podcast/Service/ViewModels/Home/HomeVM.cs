using Domain.Entities;
using Service.ViewModels.Podcast;
using Service.ViewModels.PodcastCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Home
{
    public class HomeVM
    {
        public List<PodcastCategoryAdminVM> PodcastCategories { get; set; }
        public List<PodcastAdminVM> Podcasts { get; set; }
        public List<int> FollowedPodcastIds { get; set; }
        public List<PodcastAdminVM> FollowedPodcasts { get; set; }
        public IEnumerable<Domain.Entities.Playlist> Playlists { get; set; }
    }
}
