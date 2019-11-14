using Agency.Models.Filters;
using Agency.Models.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Models.Repository
{
    public class ExperienceRepository : Repository<Experience, BaseFilter>
    {
        public ExperienceRepository(ISession session) : base(session)
        {
        }
        public IList<Experience> GetSelectedExperience(List<long> items)
        {
            var crit = session.CreateCriteria<Experience>();
            crit.Add(Restrictions.In("Id", items));
            SetupFilter(crit, null); //а это нужно? чет сомневаюсь
            return crit.List<Experience>();
        }
    }
}
