using Agency.Models.Models;
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

        public Company GetCompany(string selected)
        {
            Company company = new Company();
            foreach (var c in GetAll())
            {
                if (c.CompanyName == selected)
                    company = c;
            }
            return company;
        }

    }
}
