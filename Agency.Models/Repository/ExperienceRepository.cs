using Agency.Models.Filters;
using Agency.Models.Models;
using NHibernate;
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
    }
}
