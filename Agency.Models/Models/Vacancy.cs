﻿using Agency.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Models.Models
{
    public class Vacancy
    {
        public virtual long Id { get; set; }
        public virtual string VacancyName { get; set; }
        public virtual string VacancyDescription { get; set; }
        public virtual DateTime Starts { get; set; }
        public virtual DateTime Ends { get; set; }//тип данных - временной промежуток
        public virtual Company Company { get; set; } 
        public virtual List<Experience> Requirements { get; set; }
        public virtual Status Status { get; set; }
    }
}
