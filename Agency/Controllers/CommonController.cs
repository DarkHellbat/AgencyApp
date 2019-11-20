using Agency.Models;
using Agency.Models.Repository;
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowVacancies(FetchOptions options)
        {
            var model = new VacancyListViewModel
            {
                Role = CurrentUser.Role
            };
            switch(CurrentUser.Role)
            {
                case Models.Role.Employer:
                    {
                        model.Vacancies = employerRepository.ShowMyVacancies(CurrentUser.Id);
                       return View(model);
                    }
                case Models.Role.Admin:
                    {
                        model.Vacancies = employerRepository.GetAll();
                        return View(model);
                    }
                case Role.Jobseeker:
                    {
                        return RedirectToAction("AccessError");
                    }
                   
            }

            return View();
        }

        public ActionResult ShowCandidates()
        {
            var model = new ProfileListViewModel
            {
                Profiles = jobseekerRepository.GetAll()
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
    }
}