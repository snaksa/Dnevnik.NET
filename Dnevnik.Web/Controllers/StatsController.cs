using Dnevnik.Data;
using Dnevnik.Data.Repositories;
using Dnevnik.Data.ViewModels;
using Dnevnik.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers
{
    public class StatsController : BaseController
    {
        // GET: Stats
        public ActionResult Show(int semester = 21)
        {
            var stats = GetStats(semester, this.CurrentUser.Class_id, false);
            var zipStats = GetStats(semester, this.CurrentUser.Class_id, true);

            StatsViewModel vm = new StatsViewModel()
            {
                AllSubjects = stats,
                AllZipSubjects = zipStats,
                Semester = semester % 20
            };

            return View(vm);
        }

        private static double CalculateAverage(List<Grade> grades)
        {
            double sum = 0;
            int count = 0;
            foreach (var g in grades)
            {
                sum += g.Grade1;
                count++;
            }
            if (count == 0) return 0;
            return Math.Round(sum / grades.Count, 2);
        }

        private static List<SubjectStats> GetStats(int semester, int class_id, bool isZip)
        {
            var subjects = DB.GetClassSubjects(class_id).Where(s => s.IsZip == isZip);

            var studentsCount = StatsRepository.GetAllStudentsCount(class_id);
            var grades = StatsRepository.GetAllGrades(class_id, semester);

            List<SubjectStats> stats = new List<SubjectStats>();

            foreach (var s in subjects)
            {
                var subjectGrades = grades.Where(g => g.Subject_id == s.Id).ToList();

                SubjectStats ss = new SubjectStats()
                {
                    Subject = new SubjectVM()
                    {
                        Id = s.Id,
                        Title = s.Title
                    },
                    AllStudents = studentsCount,
                    Grades = subjectGrades,
                    StudentsWithGrades = subjectGrades.Count,
                    Average = CalculateAverage(subjectGrades)
                };
                stats.Add(ss);
            }
            return stats;
        }
    }
}