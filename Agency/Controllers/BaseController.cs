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
        protected ISession session;
        protected UserManager _userManager;

        public BaseController(UserRepository userRepository, ISession session)
        {
            this.userRepository = userRepository;
            this.session = session;
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