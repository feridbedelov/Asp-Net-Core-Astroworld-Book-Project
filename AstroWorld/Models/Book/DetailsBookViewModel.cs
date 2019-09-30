using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstroWorld.Models.Book
{
    public class DetailsBookViewModel
    {

        public int BookId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public DateTime ReleaseYear { get; set; }

        public bool Draft { get; set; }

        public ApplicationUser User { get; set; }
        public Entities.Category Category { get; set; }

        public string PhotoPath { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsAdded { get; internal set; }
    }
}
