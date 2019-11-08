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
        [Display(Name = "Ваше ФИО")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "День рождения")]
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Фото")]
        public HttpPostedFile Photo { get; set; }

        [Display(Name = "Выберите свои навыки из списка")]
        public SelectList Experience { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Фото")]
        public string ExpAsText { get; set; }
    }
}