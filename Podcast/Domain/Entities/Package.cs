using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Package:BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public DateTime PurchasedAt { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }
    }
}
