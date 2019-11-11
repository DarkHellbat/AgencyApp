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
    public class AdminController : Controller
    {
        private JobseekerRepository jobseekerRepository;
        private EmployerRepository employerRepository;
        private UserRepository userRepository;
        private UserManager userManager;
        public AdminController(JobseekerRepository jobseekerRepository, EmployerRepository employerRepository, UserRepository userRepository, UserManager userManager)
        {
            this.jobseekerRepository = jobseekerRepository;
            this.employerRepository = employerRepository;
            this.userRepository = userRepository;
            this.userManager = userManager;
        }
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult ShowCandidates()
        {
            var Profiles = jobseekerRepository.GetAll();//model = new ProfileListViewModel
            //{
            //    Profiles = jobseekerRepository.GetAll() 
            //};

            return View(Profiles);
        }

        public ActionResult ShowVacancies()
        {
            var model = new VacancyListViewModel
            {
                Vacancies = employerRepository.GetAll()
            };

            return View(model);
        }

        public ActionResult ShowUsers()
        {
            var model = new UserListViewModel
            {
                Users = userRepository.GetAll()
            };

            return View(model);
        }

        public ActionResult ChangeUser()
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
        public ActionResult ChangeUser(RegisterViewModel model)
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
                userManager.RemoveFromRole(Id, "Active");
                userManager.AddToRole(Id, "Blocked");
            }
            else
            {
                userManager.RemoveFromRole(Id, "Blocked");
                userManager.AddToRole(Id, "Active");
            }
            return RedirectToAction("Main", "Admin");
        }
    }
}