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
    public class EmployerController : Controller
    {
        private CompanyRepository companyRepository;
        private EmployerRepository employerRepository;
        private ISession session;


        public EmployerController(EmployerRepository employerRepository, CompanyRepository companyRepository, ISession session)
        {
            this.employerRepository = employerRepository;
        }
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult CreateVacancy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateVacancy(VacancyViewModel model)
        {
            var vacancy = new Vacancy 
            {
                Ends = model.Ends,
                Starts = model.Starts,
                VacancyName = model.Name,
                Status = Status.Active,
                VacancyDescription = model.Description,
                //Company = model.CompanyName.FirstOrDefault(),
                Requirements = model.Experience.SelectedValues as List<Experience>
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

        public ActionResult EditVacancy(long Id)
        {
            var vacancy = employerRepository.Load(Id);
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var c in companyRepository.GetAll())
            {
                items.Add(new SelectListItem { Text = c.CompanyName, Value = c.Id.ToString() });

            }
            var model = new VacancyViewModel
            {
                
               // CompanyName = vacancy.Company,
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
                //Company = model.CompanyName.SelectedValue,
                Requirements = model.Experience.SelectedValues as List<Experience>
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