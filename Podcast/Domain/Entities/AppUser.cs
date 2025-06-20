using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public int? PackageId { get; set; }
        public Package Package { get; set; }
        public DateTime PurchasedAt { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<AppUserPodcast> AppUserPodcasts { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
