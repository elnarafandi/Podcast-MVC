using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Podcast
{
    public class PodcastAdminVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Domain.Entities.TeamMember TeamMember { get; set; }
        public Domain.Entities.PodcastCategory PodcastCategory { get; set; }
        public ICollection<Domain.Entities.Episode> Episodes { get; set; }
    }
}
