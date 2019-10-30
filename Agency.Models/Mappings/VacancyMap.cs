using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Models.Mappings
{
    public class VacancyMap : ClassMap<Models.Vacancy>
    {
        public VacancyMap()
        {
            Id(u => u.Id).GeneratedBy.Identity();
            Map(u => u.DateRange);
            HasMany(c => c.Requirements).KeyColumn("Id");
            Map(u => u.VacancyDescription).Length(1000);
            References(u => u.Company);
            Map(u => u.VacancyName).Length(100);
        }
    }
}
