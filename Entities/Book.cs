using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class Book : EntityBase
    {

        public Book()
        {
            WishLists = new List<WishList>();
        }

        [Required]
        [MaxLength(70)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }


        [Required]
        [MaxLength(80)]
        public string Author { get; set; }

        [Required]
        public DateTime ReleaseYear { get; set; }

        [Required]
        public bool Draft { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
        [Required]
        public Category Category { get; set; }
        public string PhotoPath { get; set; }

        public List<WishList> WishLists { get; set; }

    }
}
