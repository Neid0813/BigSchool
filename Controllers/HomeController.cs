using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BigSchool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BigSchoolDbContext context = new BigSchoolDbContext();
            var upcommingCourse = context.Course.Where(n => n.DateTime > DateTime.Now).OrderBy(n => n.DateTime).ToList();
            foreach (Course item in upcommingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(item.LecturerId);
                item.Name = user.Name;
            }
            return View(upcommingCourse);
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