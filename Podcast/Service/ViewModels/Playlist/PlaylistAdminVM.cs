using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Playlist
{
    public class PlaylistAdminVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PlaylistEpisode> PlaylistEpisodes { get; set; }
    }
}
