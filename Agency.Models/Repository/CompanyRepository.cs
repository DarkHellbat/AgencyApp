using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Models.Repository
{
    public class CompanyRepository : Repository<Models.Company, Filters.BaseFilter>
    {
        public CompanyRepository(ISession session) :
                base(session)
        {

        }

    }
}
