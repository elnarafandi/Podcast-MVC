using Domain.Entities;
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
        public IEnumerable<Domain.Entities.Comment> Comments { get; set; }
        public IEnumerable<Domain.Entities.Playlist> Playlists { get; set; }
        public List<int> LikedEpisodeIds { get; set; } = new List<int>();
        public string UserId { get; set; }
    }
}
