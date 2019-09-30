using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstroWorld.Areas.Admin.Models.Book;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnitOfWork.Abstract;

namespace AstroWorld.Areas.Admin.Controllers
{   
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class BookController : Controller
    {
        private IUnitOfWork _unitOfWork;

        private UserManager<ApplicationUser> UserManager; 

        public BookController(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            UserManager = userManager;
        }

       
        public ActionResult Index()
        {
            var viewModel = new GetAllBooksViewModel()
            {
                Books = _unitOfWork.BookRepository.GetAll().Include(x => x.User).ToList()
            };

            return View(viewModel);
        }

        
        public ActionResult Details(int id)
        {
            var book = _unitOfWork.BookRepository.Get(x => x.Id == id);
            
            if (book==null)
            {
                return NotFound(); 
            }

            var viewModel = new BookDetailsViewModel()
            {
                BookId = book.Id,
                Name = book.Name,
                Description = book.Description,
                Author = book.Author,
                CreatedAt = book.CreatedAt,
                Draft = book.Draft,
                PhotoPath = book.PhotoPath,
                ReleaseYear = book.ReleaseYear,
                User  = book.User,
                Category = book.Category


            };


            return View(viewModel);
        }

       
        public ActionResult Create()
        {

            var model = new BookCreateViewModel()
            {

                CategoriesList = _unitOfWork.CategoryRepository.GetAll().
                                        Select(x => new SelectListItem
                                        {
                                            Text=x.Name,
                                            Value = x.Id.ToString()
                                        }).ToList(),
                UsersList  = UserManager.Users.ToList()
                                    .Select(y=>new SelectListItem
                                    {
                                        Text=y.Email,
                                        Value = y.Id
                                    }).ToList()
              

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = await UserManager.FindByIdAsync(model.UserId);
                var category = _unitOfWork.CategoryRepository.Get(x => x.Id == model.CategoryId);
                var newBook = new Book()
                {
                    Name = model.Name,
                    ReleaseYear =model.ReleaseYear,
                    Author = model.Author,
                    Description = model.Description,
                    Draft = model.Draft,
                    User = user,
                    Category = category
                    
                };

                 _unitOfWork.BookRepository.Add(newBook);
                if (_unitOfWork.Complete() > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

       
        public ActionResult Edit(int id)
        {
            var book = _unitOfWork.BookRepository.Find(x => x.Id == id).Include(y=>y.User).Include(v=>v.Category).FirstOrDefault();
            
            if (book==null)
            {
                return NotFound();
            }

            var viewModel = new BookUpdateViewModel()
            {
                BookId = book.Id,
                Name = book.Name,
                Description = book.Description,
                Author = book.Author,
                Draft = book.Draft,
                UserId  = book.User.Id,
                ReleaseYear = book.ReleaseYear,
                CategoryId = book.Category.Id,
                CategoriesList = _unitOfWork.CategoryRepository.GetAll().
                                        Select(x => new SelectListItem
                                        {
                                            Text=x.Name,
                                            Value = x.Id.ToString()
                                        }).ToList(),
                UsersList  = UserManager.Users.ToList()
                                    .Select(y=>new SelectListItem
                                    {
                                        Text=y.Email,
                                        Value = y.Id
                                    }).ToList()
            };

            return View(viewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BookUpdateViewModel viewModel)
        {
            try
            {
                var book = _unitOfWork.BookRepository.Find(x => x.Id == viewModel.BookId).Include(y => y.User).Include(v => v.Category).FirstOrDefault();

                if (book == null)
                {
                    return NotFound();
                }

                var user = await UserManager.FindByIdAsync(viewModel.UserId);
                var category = _unitOfWork.CategoryRepository.Get(x => x.Id == viewModel.CategoryId);
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                book.Name = viewModel.Name;
                book.Description = viewModel.Description;
                book.Author = viewModel.Author;
                book.ReleaseYear = viewModel.ReleaseYear;
                book.Draft = viewModel.Draft;
                book.User = user;
                book.Category = category;
                _unitOfWork.BookRepository.Update(book);

                if (_unitOfWork.Complete() > 0)
                {
                return RedirectToAction(nameof(Index));
                   
                }
                return View(viewModel);
            }
            catch
            {
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {

            var book = _unitOfWork.BookRepository.Get(x => x.Id == id);
            if (book==null)
            {
                return NotFound();
            }

            _unitOfWork.BookRepository.Remove(book);

            if (_unitOfWork.Complete()>0)
            {
                return RedirectToAction(nameof(Index));
            }
            return  RedirectToAction(nameof(Index));

        }

        
       
    }
}