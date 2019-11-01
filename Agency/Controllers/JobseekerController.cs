using Agency.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agency.Controllers
{
    public class JobseekerController : Controller
    {
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult CreateProfile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateProfile(ProfileModel model)
        {
            return RedirectToAction("Main", "Jobseeker");
        }
        }
}