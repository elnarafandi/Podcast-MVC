using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PlaylistEpisode:BaseEntity
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
    }
}
