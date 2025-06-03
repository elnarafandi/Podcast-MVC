using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Podcast
{
    public class PodcastDetailVM
    {
        public PodcastAdminVM Podcast { get; set; }
        public bool IsFollowing { get; set; }
    }
}
