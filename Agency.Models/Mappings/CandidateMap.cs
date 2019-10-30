using Agency.Models.Models;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Models.Mappings
{
    public class CandidateMap : ClassMap<Candidate>
    {
        public CandidateMap()
        {
            Map(c => c.Name).Length(100);
            Map(c => c.DateofBirth);
            Map(c => c.Experience);
            HasMany(c => c.Experience).KeyColumn("Id");
            References(c => c.Avatar).Cascade.SaveUpdate(); ;
        }
    }
}
