using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitOfWork.Abstract;

namespace AstroWorld.ViewComponents
{
    public class LastAddedBooksViewComponent:ViewComponent
    {
        private  IUnitOfWork unitOfWork;

        public LastAddedBooksViewComponent(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ViewViewComponentResult Invoke()
        {
            List<Entities.Book> lastAdded2 = unitOfWork.BookRepository.GetLatestBooks(2)
                .ToList();
                

            return View(lastAdded2);
        }

    }
}
