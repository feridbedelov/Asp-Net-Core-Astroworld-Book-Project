using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AstroWorld.Areas.Admin.Models.Book
{
    public class BookCreateViewModel
    {
        
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
        public string UserId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<SelectListItem> UsersList { get; set; }

        public IFormFile PhotoPath { get; set; }

        public List<SelectListItem> CategoriesList { get; set; }
    }
}
