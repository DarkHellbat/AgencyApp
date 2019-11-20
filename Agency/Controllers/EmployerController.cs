using Agency.Models;
using Agency.Models.Models;
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
    public class EmployerController : BaseController
    {
        private CompanyRepository companyRepository;
        private EmployerRepository employerRepository;

        public EmployerController(EmployerRepository employerRepository, CompanyRepository companyRepository, ExperienceRepository experienceRepository, UserRepository userRepository)
            : base(userRepository, experienceRepository)
        {
            this.employerRepository = employerRepository;
            this.companyRepository = companyRepository;
        }

        public List<SelectListItem> GetCompanies()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var c in companyRepository.GetAll())
            {
                SelectListItem item = new SelectListItem
                {
                    Text = c.CompanyName,
                    Value = c.Id.ToString()
                };
                selectList.Add(item);
            }
            return selectList;
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult CreateVacancy()
        {
            var model = new VacancyViewModel
            {
                Experience = GetExperienceLists(),
                Company = GetCompanies() 
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateVacancy(VacancyViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<long> IdList = new List<long>();
                foreach (var e in model.SelectedExperience)
                {
                    IdList.Add(Convert.ToInt64(e));
                }
                var vacancy = new Vacancy
                {
                    Ends = model.Ends,
                    Starts = model.Starts,
                    VacancyName = model.Name,
                    Status = Status.Active,
                    VacancyDescription = model.Description,
                    Company = companyRepository.Load(long.Parse(model.SelectedCompany)),
                    Requirements = experienceRepository.GetSelectedExperience(IdList)
                };
                try
                {
                    employerRepository.SaveWProcedure(vacancy, Convert.ToInt64(User.Identity.GetUserId()));
                    return RedirectToAction("Main", "employer");
                }
                catch
                {
                    return RedirectToAction("Main", "employer");
                }
            }
            else
            {
                return RedirectToAction("Main");
            }
        }

        public ActionResult EditVacancy(long Id)
        {
            var vacancy = employerRepository.Load(Id);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var c in companyRepository.GetAll())
            {
                items.Add(new SelectListItem { Text = c.CompanyName, Value = c.Id.ToString() });
                if (c == vacancy.Company)
                {
                    items.Last().Selected = true;
                }
            }

            var exp = GetExperienceLists();
            foreach (SelectListItem e in exp)
            {
                if (vacancy.Requirements.Contains(experienceRepository.Load(Convert.ToInt64(e.Value))) == true)
                    e.Selected = true;
            }

            var model = new VacancyViewModel
            {
                Experience = exp,
                Company = items,
                Description = vacancy.VacancyDescription,
                Ends = vacancy.Ends,
                Name = vacancy.VacancyName,
                Starts = vacancy.Starts
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditVacancy(VacancyViewModel model)
        {

            var vacancy = new Vacancy
            {
                Ends = model.Ends,
                Starts = model.Starts,
                VacancyName = model.Name,
                Status = Status.Active,
                VacancyDescription = model.Description,
                Company = companyRepository.Load(Convert.ToInt64(model.SelectedCompany)),
                Creator = UserManager.FindById(Convert.ToInt64(User.Identity.GetUserId())),
            };
            employerRepository.Save(vacancy);
            return RedirectToAction("Main", "employer");
        }

        public ActionResult ChangeStatus(long Id)
        {
            Vacancy vacancy = employerRepository.Load(Id);
            if (vacancy.Status == Status.Active)
                vacancy.Status = Status.Blocked;
            else
                vacancy.Status = Status.Active;
            return RedirectToAction("ShowVacancies", "employer"); 
        }
    }
}