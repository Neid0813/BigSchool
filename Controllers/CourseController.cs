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
    public class CourseController : Controller
    {
        // GET: Course


        public ActionResult Create()
        {
            BigSchoolDbContext context = new BigSchoolDbContext();
            Course objectCourse = new Course();
            objectCourse.listCategory = context.Category.ToList();

            return View(objectCourse);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objectCourse)
        {
            BigSchoolDbContext context = new BigSchoolDbContext();

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objectCourse.LecturerId = user.Id;

            context.Course.Add(objectCourse);
            context.SaveChanges();
           
            return RedirectToAction("Index","Home");
        }
    }
}