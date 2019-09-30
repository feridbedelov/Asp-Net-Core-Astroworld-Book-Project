using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class Category: EntityBase
    {

        public Category()
        {
            Books = new List<Book>();
        }


        [Required]
        [MaxLength(70)]
        public string Name { get; set; }


        [Required]
        [MaxLength(125)]
        public string Description { get; set; }

        public List<Book> Books { get; set; }



    }
}
