using Agency.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agency.Models
{
    public class ProfileModel : EntityModel<Candidate>
    {
        public ProfileModel(MultiSelectList items)
        {
            Experience = new MultiSelectList(items);
        }

        [Display(Name = "Ваше ФИО")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "День рождения")]
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Фото")]
        public HttpPostedFile Photo { get; set; }

        [Display(Name = "Выберите свои навыки из списка")]
        public MultiSelectList Experience { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Фото")]
        public string ExpAsText { get; set; }
    }

    public class ProfileListViewModel : EntityModel<List<Candidate>>
    {
        public IList<Candidate> Profiles { get; set; }
        public ProfileListViewModel()
        {
            Profiles = new List<Candidate>();
        }
    }
}