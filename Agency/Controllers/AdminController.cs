using Agency.Models;
using Agency.Models.Models;
using Agency.Models.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agency.Controllers
{
    public class AdminController : BaseController
    {
        private EmployerRepository employerRepository;
        public AdminController(EmployerRepository employerRepository, UserRepository userRepository,/* UserManager userManager, */
            ExperienceRepository experienceRepository) : base (userRepository, experienceRepository)
        {
            this.employerRepository = employerRepository;
            this.userRepository = userRepository;
        }
        public ActionResult Main()
        {
            return View();
        }

        

        public ActionResult ShowUsers()
        {
            var model = new UserListViewModel
            {
                Users = userRepository.GetAll()
            };

            return View(model);
        }

        public ActionResult EditUser()
        {
            var current = userRepository.Load(Convert.ToInt64(User.Identity.GetUserId()));
            var model = new RegisterViewModel
            {
                Email = current.UserName,
                Password = current.Password,
                Role = current.Role,
                PhoneNumber = current.PhoneNumber
            };
            return View();
        }

        [HttpPost]
        public ActionResult EditUser(RegisterViewModel model)
        {
           var user = new User
            {
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                Role = model.Role,
                UserName = model.Email
            };
            userRepository.Save(user);
            return RedirectToAction("ShowUsers", "Admin");
        }

        public ActionResult ChangeStatus()
        {
            var Id = Convert.ToInt64(User.Identity.GetUserId());
            var user = userRepository.Load(Id);
            if (user.Status == Status.Active)
            {
                user.Status = Status.Blocked;
                //UserManager.RemoveFromRole(Id, "Active");
                //UserManager.AddToRole(Id, "Blocked");
            }
            else
            {
                user.Status = Status.Active;
                //UserManager.RemoveFromRole(Id, "Blocked");
                //UserManager.AddToRole(Id, "Active");
            }
            userRepository.Save(user);
            return RedirectToAction("Main", "Admin");
        }
    }
}