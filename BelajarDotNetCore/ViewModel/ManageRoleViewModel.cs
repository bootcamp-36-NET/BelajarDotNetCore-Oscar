using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BelajarDotNetCoreAPI.ViewModel
{
    public class ManageRoleViewModel
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public List<UserRoleViewModel> Users { get; set; }
    }

    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
