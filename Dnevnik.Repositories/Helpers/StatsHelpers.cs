using Dnevnik.Data;
using Dnevnik.Repositories.Repositories;
using Dnevnik.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dnevnik.Repositories.Helpers
{
    public static class StatsHelpers
    {
        public static List<SubjectStats> GetSubjectsStats(int semester, int class_id, bool isZip)
        {
            List<SubjectVM> subjects = new List<SubjectVM>();
            if (semester == 23)
                subjects = DB.GetClassSubjects(class_id).Where(s => s.IsZip == isZip && s.Semester == semester % 20).ToList();
            else
                subjects = DB.GetClassSubjects(class_id).Where(s => s.IsZip == isZip && s.Semester == semester % 20).ToList();
            

            var studentsCount = StatsRepository.GetAllStudentsCount(class_id);
            var grades = StatsRepository.GetAllGrades(class_id, semester);

            List<SubjectStats> stats = new List<SubjectStats>();
            List<int> checkedSubjects = new List<int>();
            foreach (var s in subjects)
            {
                if (checkedSubjects.Contains(s.Id)) continue;
                else checkedSubjects.Add(s.Id);

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
    }
}