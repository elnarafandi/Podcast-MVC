using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Playlist:BaseEntity
    {
        public string Name { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<PlaylistEpisode> PlaylistEpisodes { get; set; }
    }
}
