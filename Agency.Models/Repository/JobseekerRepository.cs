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
    public class JobseekerRepository : Repository<Candidate, JobseekersFilter>
    {
        public JobseekerRepository(ISession session) :
                base(session)
            {
          
            }
        public override void SetupFilter(ICriteria crit, JobseekersFilter filter)
        {
            if (filter != null)
            {
                if (filter.Experience!=null)
                {
                    foreach (var restriction in filter.Experience)
                    {
                        crit.Add(Restrictions.IdEq(filter.Experience));
                    }
                }
                
            }
        }

        public Candidate FindProfile (long userId)
        {
            var crit = session.CreateCriteria<Candidate>();
            crit.Add(Restrictions.Eq("User.Id", userId));
            return crit.List<Candidate>().FirstOrDefault();

        }

    }
}
