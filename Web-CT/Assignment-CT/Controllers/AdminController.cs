using Assignment_CT.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Assignment_CT.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        // GET: Admin
        [Authorize(Roles = "admin")]
        public ActionResult UserList()
        {
            var usersWithRoles = db.Users.Select(u => new UserListViewModels
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                DateOfBirth = u.DateOfBirth,
                Role = db.Roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).FirstOrDefault()
            }).ToList();

            return View(usersWithRoles);
        }

        // GET: Admin/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Where(u => u.Id == id)
                       .Select(u => new UserListViewModels
                       {
                           UserId = u.Id,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Email = u.Email,
                           Address = u.Address,
                           PhoneNumber = u.PhoneNumber,
                           DateOfBirth = u.DateOfBirth,
                           Role = db.Roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).FirstOrDefault()
                       }).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /Admin/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // Post: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, FirstName = model.FirstName, LastName = model.LastName, Address = model.Address, DateOfBirth = model.DateOfBirth };
                // Create the user with a temporary password
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, model.SelectedRole);
                    return RedirectToAction("UserList");
                }
                else
                {
                    // Add the errors from the result to the model state
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);

        }

        // GET:Admin/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("UserList");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View("Delete", user);
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Where(u => u.Id == id)
                        .Select(u => new UserListViewModels
                        {
                            UserId = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            Address = u.Address,
                            PhoneNumber = u.PhoneNumber,
                            DateOfBirth = u.DateOfBirth,
                            Role = db.Roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).FirstOrDefault()
                        }).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserListViewModels model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.UserId);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.DateOfBirth = model.DateOfBirth;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;

                db.SaveChanges();

                return RedirectToAction("UserList");
            }

            return View(model);
        }


    }
}