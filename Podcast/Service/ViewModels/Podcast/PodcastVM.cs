using Service.ViewModels.PodcastCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Podcast
{
    public class PodcastVM
    {
        public IEnumerable<PodcastAdminVM> Podcasts { get; set; }
        public List<PodcastCategoryAdminVM> PodcastCategories { get; set; }
        public List<int> FollowedPodcastIds { get; set; }
        public string SearchText { get; set; }
    }
}
