using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstroWorld.Areas.Admin.Models.Identity
{
    public class ManageUserRolesViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
