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
    public class EmployerRepository : Repository<Vacancy, VacancyFilter>
    {
        public EmployerRepository(ISession session) :
                base(session)
        {

        }

        public IList<Vacancy> ShowMyVacancies(long userId)
        {
            var crit = session.CreateCriteria<Vacancy>();
            crit.Add(Restrictions.Eq("User.Id", userId));
            return crit.List<Vacancy>();
        }
    }
}
