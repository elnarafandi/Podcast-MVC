﻿using Service.ViewModels.Podcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.PodcastCategory
{
    public class PodcastCategoryAdminVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Domain.Entities.Podcast> Podcasts { get; set; }
    }
}
