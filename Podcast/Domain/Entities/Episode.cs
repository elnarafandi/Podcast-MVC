using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Episode:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image {  get; set; }
        public string Audio { get; set; }
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }
        public ICollection<EpisodeGuest> EpisodeGuests { get; set; }
        public ICollection<PlaylistEpisode> PlaylistEpisodes { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
