using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Like:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
    }
}
