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
        public CommonController(UserRepository userRepository, EmployerRepository employerRepository, ExperienceRepository experienceRepository)
            : base (userRepository, experienceRepository)
        {
            this.employerRepository = employerRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowVacancies()
        {
            switch(CurrentUser.Role)
            {
                case Models.Role.Employer:
                    {
                        var vacancies = employerRepository.ShowMyVacancies(CurrentUser.Id);
                        return View(vacancies);
                    }
                case Models.Role.Admin:
                    {
                        var model = new VacancyListViewModel
                        {
                            Vacancies = employerRepository.GetAll()
                        };

                        return View(model);
                    }
                case Role.Jobseeker:
                    {
                        return RedirectToAction("AccessError");
                    }
                   
            }

            return View();
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