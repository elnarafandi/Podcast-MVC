using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Podcast:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; } 
        public string Image {  get; set; }
        public int TeamMemberId { get; set; }
        public TeamMember TeamMember { get; set; }
        public int PodcastCategoryId { get; set; }
        public PodcastCategory PodcastCategory { get; set; }
        public ICollection<Episode> Episodes { get; set; }
        public ICollection<AppUserPodcast> AppUserPodcasts { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
