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

        public ActionResult Attending()
        {
            BigSchoolDbContext context = new BigSchoolDbContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendance.Where(m => m.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance item in listAttendances)
            {
                Course objCourse = item.Course;
                objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);   
        }

        public ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolDbContext context = new BigSchoolDbContext();
            var courses = context.Course.Where(m => m.LecturerId == currentUser.Id && m.DateTime > DateTime.Now).ToList();
            foreach (Course item in courses)
            {
                item.LectureName = currentUser.Name;
            }
            return View(courses);
        }

        [Authorize]
        public ActionResult Edit(int Id)
        {
            BigSchoolDbContext context = new BigSchoolDbContext();
            var courses = from tt in context.Course select tt;
            Course Ecourse = new Course();
            foreach (var item in courses)
            {
                if (item.Id == Id)
                {
                    Ecourse = item;
                    break;
                }
            }
            if (Ecourse == null)
            {
                return HttpNotFound();
            }
            return View(Ecourse);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "Id,lecturerId,Name,LectureName,Place,Datetime,CategoryId")] Course scourse)
        {
            BigSchoolDbContext context = new BigSchoolDbContext();
            if (ModelState.IsValid)
            {
                context.Entry(scourse).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Mine");
            }
            return View(scourse);
        }

        [Authorize]
        public ActionResult Delete(int Id)
        {
            BigSchoolDbContext context = new BigSchoolDbContext();
            var courses = from tt in context.Course select tt;
            Course Dcourse = new Course();
            foreach (var item in courses)
            {
                if (item.Id == Id)
                {
                    Dcourse = item;
                    break;
                }
            }
            if (Dcourse == null)
            {
                return HttpNotFound();
            }
            return View(Dcourse);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int Id, FormCollection collection)
        {
            BigSchoolDbContext context = new BigSchoolDbContext();
            var Dcourse = context.Course.Where(m => m.Id == Id).First();
            try
            {
                var D_tin2 = context.Attendance.Where(m => m.CourseId == Id).First();
                if (D_tin2 != null)
                {
                    context.Attendance.Remove(D_tin2);
                }
            }
            catch
            {

            }
            context.Course.Remove(Dcourse);

            context.SaveChanges();
            //var courses = from tt in context.Courses select tt;
            return RedirectToAction("Mine");
        }


    }
}