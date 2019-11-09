using Agency.Models;
using Agency.Models.Models;
using Agency.Models.Repository;
using Microsoft.AspNet.Identity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Agency.Controllers
{
    public class JobseekerController : Controller
    {
        private UserManager userManager;
        private JobseekerRepository jobseekerRepository;

        public JobseekerController( JobseekerRepository jobseekerRepository)
        {
            this.jobseekerRepository = jobseekerRepository;
        }

        public ActionResult Main()
        {
            return View();
        }
        public ActionResult CreateProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateProfile(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var file = new BinaryFile()
                {
                    Name = model.Photo.FileName,
                    //Content = model.Photo.InputStream.ToByteArray(),
                    ContentType = model.Photo.ContentType
                };
                
                Candidate candidate = new Candidate
                {
                    DateofBirth = model.DateOfBirth,
                    Name = model.Name,
                    User = await userManager.FindByIdAsync(Convert.ToInt64(User.Identity.GetUserId())),
                    //Experience =  model.Experience.ToList(),
                    Avatar = file
                };
                try
                {
                    jobseekerRepository.Save(candidate);
                    return RedirectToAction("Main", "Jobseeker");
                }
               catch
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
                    return RedirectToAction("Main", "Jobseeker");
                }
               
            }
            return RedirectToAction("Main", "Jobseeker"); //добавить оповещения
        }

        public ActionResult ChangeProfile ()
        {
            var profile = jobseekerRepository.FindProfile(Convert.ToInt64(User.Identity.GetUserId()));
            if (profile!=null)
            {
                var model = new ProfileModel
                {
                    DateOfBirth = profile.DateofBirth,
                    //Experience = profile.Experience,
                    Name = profile.Name,
                    //Photo = profile.Avatar 
                };
                return RedirectToAction("Main", "Jobseeker");
            }
            else
            {
                return RedirectToAction("CreateProfile", "Jobseeker");
            }
        }
        [HttpPost]
        public async Task< ActionResult> ChangeProfile(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var file = new BinaryFile()
                {
                    Name = model.Photo.FileName,
                    //Content = model.Photo.InputStream.ToByteArray(),
                    ContentType = model.Photo.ContentType
                };

                Candidate candidate = new Candidate
                {
                    DateofBirth = model.DateOfBirth,
                    Name = model.Name,
                    User = await userManager.FindByIdAsync(Convert.ToInt64(User.Identity.GetUserId())),
                    //Experience = model.Experience.ToList(),
                    Avatar = file
                };
                try
                {
                    jobseekerRepository.Save(candidate);
                    return RedirectToAction("Main", "Jobseeker");
                }
                catch
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
                    return RedirectToAction("Main", "Jobseeker");
                }

            }
            return RedirectToAction("Main", "Jobseeker"); //добавить оповещения
        }
        }
}