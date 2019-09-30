using AstroWorld.Models.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitOfWork.Abstract;

namespace AstroWorld.ViewComponents
{
    public class CategoryListViewComponent:ViewComponent
    {
        private IUnitOfWork UnitOfWork;
        public CategoryListViewComponent(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public ViewViewComponentResult Invoke()
        {

            var model = new CategoryListViewModel()
            {
                Categories = UnitOfWork.CategoryRepository.GetAll().ToList()
            };

            return View(model);
        }


    }
}
