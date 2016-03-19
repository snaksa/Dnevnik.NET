using Dnevnik.Data;
using Dnevnik.Repositories.AdminRepositories;
using Dnevnik.ViewModels.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers.AdminControllers
{
    public class TeachersController : AdminController
    {
        // GET: Index
        public ActionResult Show()
        {
            var classes = DB.GetClasses();
            var teachers = TeachersRepository.GetTeachers();

            TeachersViewModel vm = new TeachersViewModel()
            {
                AllTeachers = teachers,
                AllClasses = classes
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTeacher(TeachersViewModel vm)
        {
            try
            {
                TeachersRepository.AddTeacher(vm.TeacherToAdd);
                TempData["userAdded"] = "1";
            }
            catch (Exception ex)
            {
                TempData["userAdded"] = "0";
            }
            return RedirectToAction("Show");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTeachersChanges(TeachersViewModel vm)
        {
            try
            {
                TeachersRepository.UpdateTeachers(vm.AllTeachers);
                TempData["success"] = "1";
            }
            catch (Exception ex)
            {
                TempData["success"] = "0";
            }
            return RedirectToAction("Show");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            TeachersRepository.DeleteTeacher(id);
            return RedirectToAction("Show");
        }
    }
}