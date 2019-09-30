using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AstroWorld.Areas.Admin.Models.Identity
{
    public class UpdateUserViewModel
    {
        public UpdateUserViewModel()
        {
            UserRoles = new List<string>();
        }
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }

        public IList<string> UserRoles { get; set; }
    }
}
