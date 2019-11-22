using Agency.Models;
using Agency.Models.Filters;
using Agency.Models.Repository;
using Microsoft.AspNet.Identity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agency.Controllers
{
    public class CommonController : BaseController 
    {
        private EmployerRepository employerRepository;
        private JobseekerRepository jobseekerRepository;
        public CommonController(UserRepository userRepository, EmployerRepository employerRepository, ExperienceRepository experienceRepository, JobseekerRepository jobseekerRepository)
            : base (userRepository, experienceRepository)
        {
            this.employerRepository = employerRepository;
            this.jobseekerRepository = jobseekerRepository;
        }
        public ActionResult Redirect()
        {
            var role = UserManager.GetRoles(Convert.ToInt64(User.Identity.GetUserId())).SingleOrDefault();
            //иногда возникает проблема с созданием UserManager. Выглядит как проблема из коробки. Но оно работает само по себе
            return RedirectToAction("Main", String.Format("{0}", role.ToString()));
        }

        public ActionResult ShowVacancies(VacancyFilter filter, FetchOptions options)
        {
            var model = new VacancyListViewModel
            {
                Role = CurrentUser.Role
            };
            switch(CurrentUser.Role)
            {
                case Models.Role.Employer:
                    {
                        model.Vacancies = employerRepository.ShowMyVacancies(CurrentUser.Id, filter, options);
                       return View(model);
                    }
                case Models.Role.Admin:
                    {
                        model.Vacancies = employerRepository.GetAllWithSort(options);
                        return View(model);
                    }
                case Role.Jobseeker:
                    {
                        return RedirectToAction("AccessError");
                    }
                   
            }

            return View();
        }

        public ActionResult ShowCandidates(JobseekersFilter filter, FetchOptions options)
        {
            var model = new ProfileListViewModel
            {
                Profiles = jobseekerRepository.GetAllWithSort(options)
            };

            return View(model);
        }

        public ActionResult AccessError()
        {
            ViewBag.Message = @"это не те дроиды... В смысле, вы не обладаете нужными правами доступа
                                для посещения этой страницы.";
            ViewBag.Title = "Ты не пройдешь!";
            ViewData["Role"] = CurrentUser.Role.ToString();

            return View();
        }

        //public ActionResult GetSelectedItems(string selectedCompany, string selectedExperience)
        //{
        //    return PartialView("ShowVacancies", );
        //}
    }
}