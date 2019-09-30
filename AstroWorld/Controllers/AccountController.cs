using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstroWorld.Models;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SqlDatabase;

namespace AstroWorld.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationContext _dbContext;


        public AccountController(UserManager<ApplicationUser> userManager ,SignInManager<ApplicationUser> signInManager , ApplicationContext dbContext)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._dbContext = dbContext;

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded )
                {
                    return RedirectToAction("Index","Book");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }


            return View(model);
        }



        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.Email
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);


                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Index", "Book");
                }



                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }

           

            return View(model);
        }



        
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Book");
        }

    }
}