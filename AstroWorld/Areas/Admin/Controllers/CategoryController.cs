using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstroWorld.Areas.Admin.Models;
using AstroWorld.Areas.Admin.Models.Category;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnitOfWork.Abstract;

namespace AstroWorld.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        // GET: Category
        public ActionResult Index()
        {
            var viewModel = new GetCategoriesViewModel()
            {
                Categories = _unitOfWork.CategoryRepository.GetAll().ToList()
            };

            return View(viewModel);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            var category = _unitOfWork.CategoryRepository.Get(x => x.Id == id);
            if (category==null)
            {
                return NotFound();
            }

            var viewModel = new CategoryDetailsViewModel()
            {
                CategoryId = category.Id,
                CreatedAt = category.CreatedAt,
                Description =  category.Description,
                Name= category.Name
               
            };


            return View(viewModel);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCategoryViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var newCategory = new Category()
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description
                };

                _unitOfWork.CategoryRepository.Add(newCategory);
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

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            var category = _unitOfWork.CategoryRepository.Get(x => x.Id == id);
            if (category ==null)
            {
                return NotFound();
            }
                var viewModel = new UpdateCategoryViewModel()
                {
                    CategoryId = category.Id,
                    Description = category.Description,
                    Name = category.Name
                };
                return View(viewModel);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateCategoryViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }


                var toBeUpdaetdCategory = _unitOfWork.CategoryRepository.Get(x => x.Id == viewModel.CategoryId);
                toBeUpdaetdCategory.Name = viewModel.Name;
                toBeUpdaetdCategory.Description = viewModel.Description;
                _unitOfWork.CategoryRepository.Update(toBeUpdaetdCategory);
                if (_unitOfWork.Complete()>0)
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
            try
            {
                var category = _unitOfWork.CategoryRepository.Get(x => x.Id == id);
                if (category !=null)
                {
                    _unitOfWork.CategoryRepository.Remove(category);
                    if (_unitOfWork.Complete()>0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                return NotFound();
            }
            catch (Exception)
            {

                return View();
            }


            
        }

        
    }
}