using Dnevnik.Repositories.Repositories;
using Dnevnik.ViewModels.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers.AdminControllers
{
    public class SubjectsController : AdminController
    {
        // GET: Subjects
        public ActionResult Show()
        {
            var subjects = SubjectsRepository.GetAllSubjects();

            SubjectsViewModel vm = new SubjectsViewModel()
            {
                Subjects = subjects
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubject(SubjectsViewModel vm)
        {
            string title = vm.Title;
            try
            {
                if (title != null && title != string.Empty)
                {
                    SubjectsRepository.AddSubject(title, vm.IsZip);
                    TempData["subjectAdded"] = "1";
                }
            }
            catch (Exception ex)
            {
                TempData["subjectAdded"] = "0";
            }

            return RedirectToAction("Show");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSubjects(SubjectsViewModel vm)
        {
            try
            {
                if (vm.Subjects != null)
                {
                    SubjectsRepository.UpdateSubjects(vm.Subjects);
                    TempData["success"] = "1";
                }
            }
            catch (Exception ex)
            {
                TempData["success"] = "0";
            }

            return RedirectToAction("Show");
        }
    }
}