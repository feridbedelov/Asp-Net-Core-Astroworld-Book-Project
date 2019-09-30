
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {
            Books = new List<Book>();
            WishLists = new List<WishList>();
        }

        public string PhotoPath { get; set; }

        public virtual List<Book> Books { get; set; }

        public List<WishList> WishLists { get; set; }

    }
}
