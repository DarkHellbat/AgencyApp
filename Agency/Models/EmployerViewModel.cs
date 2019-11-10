using Agency.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agency.Models
{
    public class VacancyViewModel : EntityModel<Vacancy>
    {
        [Display(Name = "Название вакансии")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Начало")]
        public DateTime Starts { get; set; }
        [Display(Name = "Окончание")]
        public DateTime Ends { get; set; }

        [Display(Name = "Выберите название компании")]
        public SelectList Company { get; set; }
        [Display(Name = "Выберите требуемые навыки из списка")]
        public SelectList Experience { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Статус вакансии")]
        public string Status { get; set; }
    }

    public class VacancyListViewModel : EntityModel<List<Vacancy>>
    {
        public IList<Vacancy> Vacancies { get; set; }
        public VacancyListViewModel()
        {
            Vacancies = new List<Vacancy>();
        }
    }
}