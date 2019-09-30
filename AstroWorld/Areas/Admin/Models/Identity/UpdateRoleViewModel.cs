using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AstroWorld.Areas.Admin.Models.Identity
{
    public class UpdateRoleViewModel
    {
        public UpdateRoleViewModel()
        {
            Users = new List<string>();
        }
        public string RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
