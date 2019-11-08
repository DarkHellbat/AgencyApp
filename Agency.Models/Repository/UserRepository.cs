using Agency.Models.Filters;
using Agency.Models.Models;
using FluentNHibernate.Mapping;
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
    public class UserRepository : Repository<User, JobseekersFilter>
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

        //public User CreateUser(User user)
        //{
        //    var createUser = String.Format("INSERT INTO [User] (UserName, Password, Role, Status) VALUES ( {0}, {1}, {2}, {3}) SELECT SCOPE_IDENTITY() ", user.UserName, user.Password.GetHashCode(), user.Role, Status.Active);
        //    var result = session.CreateSQLQuery(createUser);//container.Resolve<ISession>()
        //    var a = result;
        //    result.ExecuteUpdate();
        //    return result.;

        //}

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
