using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstroWorld.Areas.Admin.Models.Identity;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.Mime.MediaTypeNames;

namespace AstroWorld.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class IdentityController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private RoleManager<IdentityRole> roleManager;

        public IdentityController(UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser> SignInManager, RoleManager<IdentityRole> RoleManager)
        {
            this.userManager = UserManager;
            this.signInManager = SignInManager;
            this.roleManager = RoleManager;
        }



        public ActionResult AllRoles()
        {
            
            return View(roleManager.Roles);
        }



        public ActionResult CreateRole()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<ActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var newRole = new IdentityRole()
                    {
                        Name = createRoleViewModel.RoleName
                    };

                    var result = await roleManager.CreateAsync(newRole);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(AllRoles));

                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                }

                return View(createRoleViewModel);

            }
            catch
            {
                return View(createRoleViewModel);
            }
        }

        


        public async Task<ActionResult> EditRole(string id)
        {

            var role = await roleManager.FindByIdAsync(id);
            if (role==null)
            {
                return NotFound();
            }

            var model = new UpdateRoleViewModel()
            {
                RoleId = role.Id,
                RoleName = role.Name

            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRole(UpdateRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = model.RoleName;
            var result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AllRoles));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);

        }

        [HttpGet]
        public async Task<ActionResult> EditUsersInRole(string id)
        {
            ViewBag.roleId = id;

            var role = await roleManager.FindByIdAsync(id);
            if (role==null)
            {
                return NotFound();
            }
            var viewModel = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if (await userManager.IsInRoleAsync(user,role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                viewModel.Add(userRoleViewModel);

               
            }
            return View(viewModel);

        }



        [HttpPost]
        public async Task<ActionResult> EditUsersInRole(List<UserRoleViewModel> models ,string id)
        {
            ViewBag.roleId = id;

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }


            for (int i = 0; i < models.Count; i++)
            {
                var user = await userManager.FindByIdAsync(models[i].UserId);

                IdentityResult result = null;

                if (models[i].IsSelected && !(await userManager.IsInRoleAsync(user,role.Name)))
                {
                    result  =  await userManager.AddToRoleAsync(user, role.Name);
                }
                else if(!models[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (models.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction(nameof(EditRole), new { Id = id });
                    }
                }

            }


            return RedirectToAction(nameof(EditRole), new { Id = id }); ;

        }



        [HttpPost]
        public async Task<ActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AllRoles));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }






        [HttpGet]
        public ActionResult AllUsers()
        {
            return View(userManager.Users);
        }


        [HttpGet]
        public ActionResult CreateUser()
        {
            var model = new CreateUserViewModel();
            model.Roles = roleManager.Roles.ToList().
                Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Name
                }).ToList();


            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(CreateUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }


                var newUser = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.Email
                };

                var result = await userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    var resultRole = await userManager.AddToRoleAsync(newUser, model.RoleName);
                    return RedirectToAction(nameof(AllUsers));
                }
                return RedirectToAction(nameof(AllUsers));


            }
            catch (Exception)
            {

                return View(model);
            }

        }



        [HttpGet]
        public async Task<ActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user==null)
            {
                return NotFound();
            }
            var roles = await userManager.GetRolesAsync(user);

            var model = new UpdateUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                UserRoles = roles 
            };

            return View(model);
        }



        [HttpPost]
        public async Task<ActionResult> EditUser(UpdateUserViewModel model)
        {


            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.UserName = model.Username;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AllUsers));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user==null)
            {
                return NotFound();
            }
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AllUsers));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }    



        [HttpGet]
        public async Task<ActionResult> EditRolesInUser(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user==null)
            {
                return NotFound();
            }

            var models = new List<ManageUserRolesViewModel>();
            foreach (var role in roleManager.Roles)
            {
                var viewModel = new ManageUserRolesViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user,role.Name))
                {
                    viewModel.IsSelected = true;
                }
                else
                {
                    viewModel.IsSelected = false;
                }

                models.Add(viewModel);

            }
            return View(models);
        }


        [HttpPost]
        public async Task<ActionResult> EditRolesInUser(List<ManageUserRolesViewModel> model,string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);

            if (user==null)
            {
                return NotFound();
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected == true).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                return View(model);
            }


            return RedirectToAction(nameof(EditUser),new {Id = userId});
        }


    }
}