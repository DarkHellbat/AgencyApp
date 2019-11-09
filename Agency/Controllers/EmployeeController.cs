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
    public class employerController : Controller
    {
        private CompanyRepository companyRepository;
        private EmployerRepository employerRepository;
        private ISession session;


        public employerController(EmployerRepository employerRepository, CompanyRepository companyRepository, ISession session)
        {
            this.employerRepository = employerRepository;
        }
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult ShowMyVacancies()
        {
            var vacancies = employerRepository.ShowMyVacancies(Convert.ToInt64(User.Identity.GetUserId()));
            return View(vacancies);
        }

        public ActionResult CreateVacancy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateVacancy(VacancyViewModel model)
        {
            var vacancy = new Vacancy //возможно имеет смысл вынести этот код в метод репозитория? 
            {//т.к. он повторяется
                Ends = model.Ends,
                Starts = model.Starts,
                VacancyName = model.Name,
                Status = Status.Active,
                VacancyDescription = model.Description,
                //Company = model.CompanyName.FirstOrDefault(),
                Requirements = model.Experience.SelectedValue as List<Experience>
            };
            try
            {
                var result = session.CreateSQLQuery("exec sp_InsertVacancy :VacancyName, :VacancyDescription, :Starts, :Ends, :User_id, :Status, :Company_id")
                    .SetParameter("VacancyName", vacancy.VacancyName)
                    .SetParameter("VacancyDescription", vacancy.VacancyDescription)
                    .SetParameter("Starts", vacancy.Starts)
                    .SetParameter("Ends", vacancy.Ends)
                    .SetParameter("User_id", Convert.ToInt64( User.Identity.GetUserId()))
                    .SetParameter("Status", vacancy.Status)
                    .SetParameter("Company_id", vacancy.Company.Id)
                    ;
                result.ExecuteUpdate();
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
            //model.CompanyName.Append()
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
                Requirements = model.Experience.SelectedValue as List<Experience>
            };
            employerRepository.Save(vacancy);
            return RedirectToAction("Main", "employer");
        }
        }
}