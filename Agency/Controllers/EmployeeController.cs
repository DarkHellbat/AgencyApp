using Agency.Models.Repository;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agency.Controllers
{
    public class EmployeeController : BaseController
    {
        private UserRepository userRepository;


        public EmployeeController(UserRepository userRepository, ISession session) : base(userRepository, session)
        {
           // this.userRepository = userRepository;
        }
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}