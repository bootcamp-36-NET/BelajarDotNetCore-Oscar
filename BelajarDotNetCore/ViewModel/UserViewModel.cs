using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public List<string> RoleName { get; set; }

        public bool EmailConfirmed { get; set; }

        public string JWToken { get; set; }
    }
}
