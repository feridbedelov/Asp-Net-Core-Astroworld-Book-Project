using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitOfWork.Abstract;

namespace AstroWorld.ViewComponents
{
    public class RandomPostViewComponent : ViewComponent
    {
        private IUnitOfWork UnitOfWork;
        public RandomPostViewComponent(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public ViewViewComponentResult Invoke()
        {
            return View(UnitOfWork.BookRepository.GetRandomBook().FirstOrDefault());
        }
    }
}
