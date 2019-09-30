using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstroWorld.Areas.Admin.Models.Category
{
    public class GetCategoriesViewModel
    {
        public List<Entities.Category> Categories { get; set; } 
    }
}
