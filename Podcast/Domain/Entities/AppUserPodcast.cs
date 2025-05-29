using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppUserPodcast:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }
    }
}
