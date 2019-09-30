using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AstroWorld.Areas.Admin.Models.Book
{
    public class BookUpdateViewModel
    {
        public int BookId { get; set; }

        public string Name { get; set; }

       
        public string Description { get; set; }


        public string Author { get; set; }

        
        public DateTime ReleaseYear { get; set; }

        
        public bool Draft { get; set; }

        public List<SelectListItem> UsersList { get; set; }

        public List<SelectListItem> CategoriesList { get; set; }

        public IFormFile PhotoPath { get; set; }


        [Required]
        public string UserId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
