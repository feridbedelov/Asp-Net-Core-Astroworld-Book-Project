using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AstroWorld.Areas.Admin.Models.Book
{
    public class BookDetailsViewModel
    {

        public int BookId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public DateTime ReleaseYear { get; set; }

        public bool Draft { get; set; }

        public ApplicationUser  User { get; set; }
        public Entities.Category Category { get; set; }

        public string PhotoPath { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
