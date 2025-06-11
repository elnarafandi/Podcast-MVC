using Service.ViewModels.Podcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.PodcastCategory
{
    public class PodcastCategoryVM
    {
        public IEnumerable<PodcastAdminVM> Podcasts { get; set; }
        public PodcastCategoryAdminVM PodcastCategory{ get; set; }
        public List<int> FollowedPodcastIds { get; set; }
        public int PodcastCount { get; set; }
    }
}
