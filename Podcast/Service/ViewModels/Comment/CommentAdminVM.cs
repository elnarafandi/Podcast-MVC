using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Comment
{
    public class CommentAdminVM
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Domain.Entities.AppUser AppUser { get; set; }
        public Domain.Entities.Podcast Podcast { get; set; }
    }
}
