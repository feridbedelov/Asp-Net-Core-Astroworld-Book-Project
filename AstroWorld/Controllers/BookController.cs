using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AstroWorld.Models.Book;
using AstroWorld.Models.WishList;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnitOfWork.Abstract;

namespace AstroWorld.Controllers
{
    public class BookController : Controller
    {
        private IUnitOfWork UnitOfWork;

        private SignInManager<ApplicationUser> SignInManager;

        private UserManager<ApplicationUser> UserManager;

        public BookController(IUnitOfWork unitOfWork,SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager)
        {
            UnitOfWork = unitOfWork;
            SignInManager = signInManager;
            UserManager = userManager;
        }

        public ActionResult Index()
        {
            
            var viewModel = new GetBooksViewModel()
            {
                Books = UnitOfWork.BookRepository.GetAll()
                .Where(z=>z.Draft==false)
                .Include(x=>x.User)
                .Include(y=>y.Category)
                .OrderByDescending(v => v.CreatedAt)
                .ToList()
                
            };

            return View(viewModel);
        }


        private bool isAdded(int bookid)
        {
            if (GetUserId()==null)
            {
                return true;
            }
            return  UnitOfWork.WishListRepository
                .GetWishLists(x => x.UserId == GetUserId() && x.BookId == bookid).FirstOrDefault() == null ? false : true;
        }

        
        public ActionResult Details(int id)
        {
            var book = UnitOfWork.BookRepository.Find(x=>x.Id == id).Include(y=>y.User).Include(z=>z.Category).FirstOrDefault();

            if (book == null)
            {
                return NotFound();
            }

            

            var viewModel = new DetailsBookViewModel()
            {
                BookId = book.Id,
                Name = book.Name,
                Description = book.Description,
                Author = book.Author,
                CreatedAt = book.CreatedAt,
                Draft = book.Draft,
                PhotoPath = book.PhotoPath,
                ReleaseYear = book.ReleaseYear,
                User = book.User,
                Category = book.Category,
                IsAdded=isAdded(id)


            };


            return View(viewModel);



        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CreateBookViewModel()
            {
                CategoriesList = UnitOfWork.CategoryRepository.GetAll()
                .Select(x=>new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult GetBookByCategory(int id)
        {

            var viewModel = new GetBooksViewModel()
            {
                Books = UnitOfWork.BookRepository.GetByCategoryId(id)
                .Where(x=>x.Draft==false)
                .Include(y=>y.User)
                .Include(z=>z.Category)
                .OrderByDescending(v=>v.CreatedAt)
                .ToList()
            };

            return View("Index",viewModel);
        }

        private string GetUserId()
        {
            if (SignInManager.IsSignedIn(User))
            {
                return User.FindFirst(ClaimTypes.NameIdentifier).Value;

            }
            else
            {
                return null;

            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateBookViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var category = UnitOfWork.CategoryRepository.Get(x => x.Id == model.CategoryId);
                var user = await UserManager.FindByIdAsync(GetUserId());
                var newBook = new Book()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Author = model.Author,
                    ReleaseYear = model.ReleaseYear,
                    Draft = model.Draft,
                    Category = category,
                    User = user
                    
                };

                UnitOfWork.BookRepository.Add(newBook);
                if (UnitOfWork.Complete()>0)
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

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {

            var book = UnitOfWork.BookRepository.Find(x => x.Id == id).Include(y => y.User).Include(z => z.Category).FirstOrDefault();

            if (book == null)
            {
                return NotFound();
            }
            if (book.User.Id != GetUserId())
            {
                return Forbid();
            }
            var viewModel = new UpdateBookViewModel()
            {
                BookId = book.Id,
                Name = book.Name,
                Description = book.Description,
                Draft = book.Draft,
                Author = book.Author,
                ReleaseYear = book.ReleaseYear,
                CategoryId = book.Category.Id,
                UserId = book.User.Id,
                CategoriesList = UnitOfWork.CategoryRepository.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };



            return View(viewModel);
        }

        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateBookViewModel viewModel)
        {
            try
            {
                var book = UnitOfWork.BookRepository.Find(x => x.Id == viewModel.BookId).Include(y => y.User).Include(v => v.Category).FirstOrDefault();

                if (book == null)
                {
                    return NotFound();
                }
                if (book.User.Id != GetUserId())
                {
                    return Forbid();
                }
                var category = UnitOfWork.CategoryRepository.Get(x => x.Id == viewModel.CategoryId);
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                book.Name = viewModel.Name;
                book.Description = viewModel.Description;
                book.Author = viewModel.Author;
                book.ReleaseYear = viewModel.ReleaseYear;
                book.Draft = viewModel.Draft;
                book.Category = category;
                UnitOfWork.BookRepository.Update(book);
                if (UnitOfWork.Complete() > 0)
                {
                    return RedirectToAction(nameof(Details), new {id = book.Id } );

                }
                return View(viewModel);
            }
            catch
            {
                return View(viewModel);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var book = UnitOfWork.BookRepository.Find(x => x.Id == id).Include(y => y.User).Include(v => v.Category).FirstOrDefault();

            if (book == null)
            {
                return NotFound();
            }

            if (book.User.Id != GetUserId())
            {
                return Forbid();
            }

            UnitOfWork.BookRepository.Remove(book);
            if (UnitOfWork.Complete()>0)
            {
                return RedirectToAction(nameof(Index));
            }


            return View();
        }

        [HttpGet]
        public ActionResult GetBookByUser(string id)
        {
            
            var viewModel = new GetBooksViewModel()
            {
                Books = UnitOfWork.BookRepository.GetByUserId(id)
                .Where(x => x.Draft == false)
                .Include(y => y.User)
                .Include(z => z.Category)
                .OrderByDescending(v => v.CreatedAt)
                .ToList()
            };

            return View("Index", viewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetBookByCuurentUser()
        {
            var viewModel = new GetBooksViewModel()
            {
                Books = UnitOfWork.BookRepository.Find(x=>x.User.Id == GetUserId())
                .Include(y => y.User)
                .Include(z => z.Category)
                .OrderByDescending(v => v.CreatedAt)
                .ToList()
            };

            return View("Index", viewModel);
        }

        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddToWishList(int bookId)
        {
            if (isAdded(bookId)==true)
            {
                return NoContent();
            }

            var book = UnitOfWork.BookRepository.Get(x => x.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            if (book.Draft==true)
            {
                return NoContent();
            }
            var user = await UserManager.FindByIdAsync(GetUserId());

            var wishlist = new WishList()
            {
                Book = book,
                User = user
            };

            UnitOfWork.WishListRepository.Add(wishlist);
            if (UnitOfWork.Complete()>0)
            {
                return RedirectToAction(nameof(Details), new { id = book.Id });
            };

            return View("Error");
        }




        [Authorize]
        [HttpGet]
        public ActionResult GetWishlistOfCurrentUser()
        {
            var userId = GetUserId();
            var wishlists = UnitOfWork.WishListRepository.GetWishLists(x => x.UserId == userId);
            var viewModel = new GetWishlistViewModel()
            {
                WishLists = wishlists
                .Include(x=>x.Book).ThenInclude(y=>y.Category)
                .Include(z=>z.User)
                .OrderByDescending(v =>v.Book.CreatedAt )
                .ToList()
            };
            
            return View(viewModel);
        }



        

        [Authorize]
        [HttpPost]
        public ActionResult RemoveFromWishList(int bookId)
        {
            if (isAdded(bookId)==true)
            {
                var wishlist = UnitOfWork.WishListRepository
                    .GetWishLists(x => x.BookId == bookId && x.UserId == GetUserId()).FirstOrDefault();

                if (wishlist==null)
                {
                    return NotFound();
                }

                UnitOfWork.WishListRepository.Remove(wishlist);
                if (UnitOfWork.Complete()>0)
                {
                    return RedirectToAction(nameof(GetWishlistOfCurrentUser));
                }
                return View();
            }

            return View();
        }

    }
}