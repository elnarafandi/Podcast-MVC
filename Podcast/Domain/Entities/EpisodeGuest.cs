using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EpisodeGuest:BaseEntity
    {
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
    }
}
