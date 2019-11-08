using Agency.Models.Models;
using Agency.Models.Repository;
using Microsoft.AspNet.Identity.Owin;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agency.Controllers
{
    public class BaseController : Controller
    {
        protected UserRepository userRepository;
        protected ISession Session;

        public BaseController(UserRepository repository, ISession session)
        {
            repository = userRepository;
            session = Session;
        }

        public UserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
        }

        public User CurrentUser
        {
            get { return userRepository.GetCurrentUser(User); }
        }
    }
}