using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class WishList
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
