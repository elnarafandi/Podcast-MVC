using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TeamMember : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Information { get; set; }
        public string SocialMedia { get; set; }
        public ICollection<Podcast> Podcasts { get; set; }
    }
}
