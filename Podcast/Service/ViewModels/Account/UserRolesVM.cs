using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels.Account
{
    public class UserRolesVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PackageId { get; set; }
        public string Image { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}
