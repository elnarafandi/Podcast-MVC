using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Episode
{
    public class EpisodeAdminVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Audio { get; set; }
        public Domain.Entities.Podcast Podcast { get; set; }
        public ICollection<Domain.Entities.EpisodeGuest> EpisodeGuests { get; set; }
    }
}
