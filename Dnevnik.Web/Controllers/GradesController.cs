using Dnevnik.Data;
using Dnevnik.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers
{
    public class GradesController : BaseController
    {
        [HttpGet]
        public ActionResult Show(int? semester, int? id)
        {
            if (semester == null)
            {
                var subjects = DB.GetClassSubjects(this.CurrentUser.Class_id);
                return View(subjects);
            }
            else if (semester == 1)
            {
                return ShowFirstSemesterGrades(1, id);
            }
            else
            {
                return ShowSecondSemesterGrades(2, id);
            }
            return View();
        }

        [HttpGet]
        public ActionResult ShowFirstSemesterGrades(int class_id, int? subject_id)
        {
            var grades = GradesRepository.GetGrades(this.CurrentUser.Class_id, subject_id);
            foreach (var grade in grades)
            {
                grade.Grades = new string[7];
                grade.Grades[0] = String.Join(",", grade.GradesArray.Where(g => g.Month == 9).Select(g => g.Grade).ToArray());
                grade.Grades[1] = String.Join(",", grade.GradesArray.Where(g => g.Month == 10).Select(g => g.Grade).ToArray());
                grade.Grades[2] = String.Join(",", grade.GradesArray.Where(g => g.Month == 11).Select(g => g.Grade).ToArray());
                grade.Grades[3] = String.Join(",", grade.GradesArray.Where(g => g.Month == 12).Select(g => g.Grade).ToArray());
                grade.Grades[4] = String.Join(",", grade.GradesArray.Where(g => g.Month == 1).Select(g => g.Grade).ToArray());
                grade.Grades[5] = String.Join(",", grade.GradesArray.Where(g => g.Month == 21).Select(g => g.Grade).ToArray());
            }

            var vm = new GradesViewModel()
            {
                Students = grades,
                Subject_id = subject_id,
                Semester = 1
            };

            return View("FirstSemesterGrades", vm);
        }

        public ActionResult ShowSecondSemesterGrades(int class_id, int? subject_id)
        {
            var grades = GradesRepository.GetGrades(this.CurrentUser.Class_id, subject_id);
            foreach (var grade in grades)
            {
                grade.Grades = new string[7];
                grade.Grades[0] = String.Join(",", grade.GradesArray.Where(g => g.Month == 2).Select(g => g.Grade).ToArray());
                grade.Grades[1] = String.Join(",", grade.GradesArray.Where(g => g.Month == 3).Select(g => g.Grade).ToArray());
                grade.Grades[2] = String.Join(",", grade.GradesArray.Where(g => g.Month == 4).Select(g => g.Grade).ToArray());
                grade.Grades[3] = String.Join(",", grade.GradesArray.Where(g => g.Month == 5).Select(g => g.Grade).ToArray());
                grade.Grades[4] = String.Join(",", grade.GradesArray.Where(g => g.Month == 6).Select(g => g.Grade).ToArray());
                grade.Grades[5] = String.Join(",", grade.GradesArray.Where(g => g.Month == 22).Select(g => g.Grade).ToArray());
                grade.Grades[6] = String.Join(",", grade.GradesArray.Where(g => g.Month == 23).Select(g => g.Grade).ToArray());
            }

            var vm = new GradesViewModel()
            {
                Students = grades,
                Subject_id = subject_id,
                Semester = 2
            };

            return View("SecondSemesterGrades", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveGrades(GradesViewModel vm)
        {
            try
            {
                GradesRepository.AddGradesToDB(vm.Students, vm.Semester, vm.Subject_id.Value);
                TempData["success"] = "1";
            }
            catch(Exception ex)
            {
                TempData["success"] = "0";
            }
            return RedirectToAction("Show", new { semester = vm.Semester, id = vm.Subject_id });
        }
    }
}