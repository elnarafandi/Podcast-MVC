using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Playlist
{
    public class PlaylistVM
    {
        public PlaylistAdminVM Playlist {  get; set; }
        public List<int> LikedEpisodeIds { get; set; }
        public Domain.Entities.AppUser User { get; set; }
    }
}
