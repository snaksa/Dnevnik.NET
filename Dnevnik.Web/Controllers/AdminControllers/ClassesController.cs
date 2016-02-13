using Dnevnik.Data;
using Dnevnik.Data.AdminRepositories;
using Dnevnik.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers.AdminControllers
{
    public class ClassesController : AdminController
    {
        // GET: Classes
        public ActionResult Show()
        {
            var classes = ClassesRepository.GetClasses();
            var vm = new ClassesViewModel()
            {
                AllClasses = classes
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddClass(ClassesViewModel vm)
        {
            try
            {
                ClassesRepository.AddClass(vm.ClassToAdd);
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
        public ActionResult UpdateClasses(ClassesViewModel vm)
        {
            try
            {
                ClassesRepository.UpdateClasses(vm.AllClasses);
                TempData["success"] = "1";
            }
            catch (Exception ex)
            {
                TempData["success"] = "0";
            }

            return RedirectToAction("Show");
        }
    }
}