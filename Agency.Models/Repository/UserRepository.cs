using Agency.Models.Filters;
using Agency.Models.Models;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Agency.Models.Repository
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(ISession session) :
            base(session)
        {
        }
        public User FindByLogin(string login)
        {
            var crit = session.CreateCriteria<User>();
            crit.Add(Restrictions.Eq("Login", login));
            try
            {
                return crit.List<User>().FirstOrDefault();
            }
            catch
            {
                User user = null;
                return user;
            }
        }

        public IList<Candidate> Find(JobseekersFilter filter, FetchOptions options = null)
        {
            var crit = session.CreateCriteria<Candidate>();
            SetupFilter(filter, crit);
            SetFetchOptions(crit, options);
            return crit.List<Candidate>();
        }

        protected virtual void SetupFilter(ICriteria crit, JobseekersFilter filter)
        {
            if (filter != null)
            {
                //if (!string.IsNullOrEmpty(filter.Name))
                //{
                //    crit.Add(Restrictions.Like("Name", filter.Name, MatchMode.Anywhere));
                //}
                //if (filter.CreationDate != null)
                //{
                //    if (filter.CreationDate.From.HasValue)
                //    {
                //        crit.Add(Restrictions.Ge("Created", filter.CreationDate.From.Value));
                //    }
                //    if (filter.CreationDate.To.HasValue)
                //    {
                //        crit.Add(Restrictions.Le("Created", filter.CreationDate.To.Value));
                //    }
                //}
                //if (filter.ChangingDate != null)
                //{
                //    if (filter.ChangingDate.From.HasValue)
                //    {
                //        crit.Add(Restrictions.Ge("Changed", filter.CreationDate.From.Value));
                //    }
                //    if (filter.CreationDate.To.HasValue)
                //    {
                //        crit.Add(Restrictions.Le("Changed", filter.CreationDate.To.Value));
                //    }
                //}
            }
        }

        public User GetCurrentUser(IPrincipal user = null)
        {
            user = user ?? (HttpContext.Current != null ? HttpContext.Current.User : Thread.CurrentPrincipal);
            if (user == null || user.Identity == null)
            {
                return null;
            }
            var currentUserId = user.Identity.GetUserId();
            long userId;
            if (string.IsNullOrEmpty(currentUserId) || !long.TryParse(currentUserId, out userId))
            {
                return null;
            }
            return session.Get<User>(userId);
        }

    }
}
