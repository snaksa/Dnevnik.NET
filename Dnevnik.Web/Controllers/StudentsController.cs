namespace Dnevnik.Web.Controllers
{
    using Dnevnik.Data;
    using System.Data.Entity;
    using System.Web.Mvc;
    using System.Linq;
    using System;
    using Dnevnik.Repositories.Repositories;
    using Dnevnik.ViewModels.Web;

    public class StudentsController : BaseController
    {
        [HttpGet]
        public ActionResult Show()
        {
            var students = StudentsRepository.GetAllStudents(this.CurrentUser.Class_id);

            StudentsViewModel vm = new StudentsViewModel()
            {
                AllStudents = students
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStudentChanges(StudentsViewModel data)
        {
            try
            {
                StudentsRepository.SaveStudents(data.AllStudents);
                TempData["success"] = "1";
            }
            catch (Exception ex)
            {
                //Log to the database
                TempData["success"] = "0";
            }
            return RedirectToAction("Show");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStudent(StudentsViewModel data)
        {
            try
            {
                StudentsRepository.AddStudent(data.StudentToAdd, this.CurrentUser.Class_id);
                TempData["userAdded"] = "1";
            }
            catch (Exception ex)
            {
                TempData["userAdded"] = "0";
                //log to the databse
            }

            return RedirectToAction("Show");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                StudentsRepository.DeleteStudent(id, this.CurrentUser.Class_id);
                TempData["success"] = "1";
            }
            catch (Exception ex)
            {
                //Log to the database
                TempData["success"] = "0";
            }
            return RedirectToAction("Show");
        }
    }
}