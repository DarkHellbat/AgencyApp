using Agency.Models;
using Agency.Models.Models;
using Agency.Models.Repository;
using Microsoft.AspNet.Identity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Agency.Controllers
{
    public class JobseekerController : BaseController
    {
        private UserManager userManager;
        private JobseekerRepository jobseekerRepository;
        private ExperienceRepository experienceRepository;
        private BinaryFileRepository fileRepository;

        public JobseekerController( JobseekerRepository jobseekerRepository, ExperienceRepository experienceRepository, BinaryFileRepository fileRepository, UserRepository userRepository) : base (userRepository)
        {
            this.jobseekerRepository = jobseekerRepository;
            this.experienceRepository = experienceRepository;
            this.fileRepository = fileRepository;
        }

        public ActionResult Main()
        {
            return View();
        }

        public List<SelectListItem> GetExperienceLists()
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
           return listItems;
        }

        

        public ActionResult CreateProfile()
        {
            var model = new ProfileModel
            {
                Experience = GetExperienceLists()
            };   
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> CreateProfile(ProfileModel model)
        {
            //if (ModelState.IsValid)
            //{
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var file = new BinaryFile
                {
                    Name = model.Photo.FileName,
                    Path = Path.Combine(path, @"App_Data\Files", DateTime.Now.ToString().Replace("/", "_").Replace(":", "_") + model.Photo.FileName),
                    //Content = model.Photo.InputStream.ToByteArray(),
                    ContentType = model.Photo.ContentType
                };
            if (!Directory.Exists(file.Path))
            {
                Directory.CreateDirectory(Path.Combine(path, @"App_Data\Files"));
                 
            }
            using (var fileStream = System.IO.File.Create(file.Path))
                {
                    model.Photo.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
                    model.Photo.InputStream.CopyTo(fileStream);
                }
            fileRepository.Save(file);
            List<long> IdList = new List<long>();
            var request = Request.Form["Experience"];

                foreach (var e in model.Experience)
            {
                if (e.Selected == true)
                {
                    IdList.Add(Convert.ToInt64(e.Value));
                }
            }
            Candidate candidate = new Candidate
                {
                    DateofBirth = model.DateOfBirth,
                    Name = model.Name,
                    User = UserManager.FindById(Convert.ToInt64(User.Identity.GetUserId())),
                    Experience = experienceRepository.GetSelectedExperience(IdList),
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
               
            //}
            return RedirectToAction("Main", "Jobseeker"); //добавить оповещения
        }

        public ActionResult EditProfile ()
        {
            var profile = jobseekerRepository.FindProfile(Convert.ToInt64(User.Identity.GetUserId()));
            if (profile!=null)
            {
                var exp = GetExperienceLists();
                foreach (SelectListItem e in exp) //profile.Experience)
                {
                    if (profile.Experience.Contains(experienceRepository.Load(Convert.ToInt64(e))) == true)
                        e.Selected = true;
                }
                var model = new ProfileModel
                {
                    Experience = GetExperienceLists(),
                    DateOfBirth = profile.DateofBirth,
                    //Experience = exp,
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

                Candidate candidate = new Candidate
                {
                    DateofBirth = model.DateOfBirth,
                    Name = model.Name,
                    User = await userManager.FindByIdAsync(Convert.ToInt64(User.Identity.GetUserId())),
                    Avatar = file
                };
                foreach (var e in model.Experience)
                {
                    if (e.Selected == true)
                    {
                        Experience experience = experienceRepository.Load(Convert.ToInt64(e.Value)); //оно сработает???
                        candidate.Experience.Add(experience);
                    }
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