﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Guest
{
    public class GuestAdminVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Information { get; set; }
        public string SocialMedia { get; set; }
    }
}
