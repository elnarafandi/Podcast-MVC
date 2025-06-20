using Service.ViewModels.Podcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Episode
{
    public class EpisodeVM
    {
        public List<EpisodeAdminVM> Episodes { get; set; }
        public List<PodcastAdminVM> Podcasts { get; set; }
    }
}
