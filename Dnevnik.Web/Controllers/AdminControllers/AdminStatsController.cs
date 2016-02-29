using Dnevnik.Data;
using Dnevnik.Data.Repositories;
using Dnevnik.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers.AdminControllers
{
    public class AdminStatsController : AdminController
    {
        public ActionResult Show(int class_id = -1)
        {
            if (class_id == -1)
            {
                var classes = DB.GetClasses();
                return View(classes);
            }
            else
            {
                var students = StudentsRepository.GetAllStudents(class_id);
                var subjects = DB.GetClassSubjects(class_id);

                if (students.Count == 0 || subjects.Count == 0) return RedirectToAction("Show", "Teachers");

                string studClass = students[0].Class.Number.ToString();
                if (students[0].Class.Letter == 1) studClass += "a";
                else if (students[0].Class.Letter == 2) studClass += "б";
                else if (students[0].Class.Letter == 3) studClass += "в";
                else if (students[0].Class.Letter == 4) studClass += "г";
                else if (students[0].Class.Letter == 5) studClass += "д";

                AdminStatsViewModel vm = new AdminStatsViewModel()
                {
                    Students = students,
                    Subjects = subjects,
                    SelectedClass = studClass
                };
                return View("ShowClass", vm);
            }

        }
    }
}