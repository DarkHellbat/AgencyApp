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
        private ExperienceRepository experienceRepository;

        public JobseekerController( JobseekerRepository jobseekerRepository, ExperienceRepository experienceRepository)
        {
            this.jobseekerRepository = jobseekerRepository;
            this.experienceRepository = experienceRepository;
        }

        public ActionResult Main()
        {
            return View();
        }

        public MultiSelectList GetExperienceLists()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var e in experienceRepository.GetAll())
            {
                SelectListItem item = new SelectListItem
                {
                    Text = e.Skill,
                    Value = e.Id.ToString()
                }; 
                listItems.Add(item);
            }
            MultiSelectList items = new MultiSelectList(listItems.OrderBy(i => i.Text));
            return items;
        }

        public ActionResult CreateProfile()
        {
            var model = new ProfileModel(GetExperienceLists());
            return View(model);
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

        public ActionResult EditProfile ()
        {
            var profile = jobseekerRepository.FindProfile(Convert.ToInt64(User.Identity.GetUserId()));
            if (profile!=null)
            {
                var exp = GetExperienceLists();
                foreach (SelectListItem e in exp.Items) //profile.Experience)
                {
                    if (profile.Experience.Contains(experienceRepository.Load(Convert.ToInt64(e))) == true)
                        e.Selected = true;
                }
                var model = new ProfileModel(GetExperienceLists())
                {
                    DateOfBirth = profile.DateofBirth,
                    Experience = exp,
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
        public async Task<ActionResult> EditProfile(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var file = new BinaryFile()
                {
                    Name = model.Photo.FileName,
                    //Content = model.Photo.InputStream.ToByteArray(),
                    ContentType = model.Photo.ContentType
                };
                var n = model.Experience.SelectedValues;
                Candidate candidate = new Candidate
                {
                    DateofBirth = model.DateOfBirth,
                    Name = model.Name,
                    User = await userManager.FindByIdAsync(Convert.ToInt64(User.Identity.GetUserId())),
                    Avatar = file
                };
                foreach (var e in model.Experience.SelectedValues)
                {
                    Experience experience = experienceRepository.Load(Convert.ToInt64(e)); //оно сработает???
                    candidate.Experience.Add(experience);
                }
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