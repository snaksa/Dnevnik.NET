using Dnevnik.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.Data
{
    public static class GradesRepository
    {
        public static List<StudentGradesViewModel> GetGrades(int class_id, int? subject_id)
        {
            var db = new DnevnikEntities();
            var grades = db.Students
                .Where(s => s.Class_id == class_id)
                .OrderBy(s => s.Number)
                .Select(s => new StudentGradesViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Number = s.Number,
                    //Grades = s.Grades.Where(g => g.Subject_id == subject_id).ToList()
                    GradesArray = s.Grades
                    .Where(g => g.Subject_id == subject_id)
                    .Select(g => new StudentGrade
                    {
                        Month = g.Grade_month,
                        Grade = g.Grade1
                    }).ToList()
                })
                .ToList();
            return grades;
        }

        public static void DeleteGradesBySubject(int semester, int student_id, int subject_id)
        {
            List<int> f = new List<int>() { 9, 10, 11, 12, 1, 21 };
            List<int> s = new List<int>() { 2, 3, 4, 5, 6, 22, 23 };
            List<int> toCheck = new List<int>();
            if (semester == 1) toCheck = f;
            else toCheck = s;

            using (var db = new DnevnikEntities())
            {
                //db.Database.ExecuteSqlCommand("DELETE FROM Grades WHERE Subject_id = @subject_id AND Student_id = @student_id",
                //    new SqlParameter("@subject_id", subject_id),
                //    new SqlParameter("@student_id", student_id));
                db.Grades.RemoveRange(db.Grades
                        .Where(x =>
                        toCheck.Contains(x.Grade_month)
                        && x.Subject_id == subject_id
                        && x.Student_id == student_id));
                db.SaveChanges();
            }
        }

        public static void AddGradesToDB(List<StudentGradesViewModel> students, int semester, int subject_id)
        {
            var db = new DnevnikEntities();

            try
            {
                //5 meseca za srok + 1 za srochna ocenka
                //ako e vtori semester + 1 za godishna ocenka
                int len = 0;
                if (semester == 1) len = 6;
                else len = 7;

                //za vseki uchenik vzemame ocenkite po mesec, razbivame gi i gi vkarvame v bazata
                foreach (var s in students)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (s.Grades[i] != string.Empty)
                        {
                            var grades = s.Grades[i].Split(new char[] { ',', ';' });//remove empty entries
                            foreach (var grade in grades)
                            {
                                if (grade.Trim() == string.Empty) continue;

                                int month = ConvertMonth(i, semester);

                                Grade g = new Grade
                                {
                                    Grade_month = month,
                                    Grade1 = System.Int32.Parse(grade.Trim()),
                                    Student_id = s.Id,
                                    Subject_id = subject_id,
                                };

                                db.Grades.Add(g);
                                db.Entry(g).State = EntityState.Added;
                            }
                        }
                    }
                }

                //iztrivame starite ocenki predi da vkarame novite
                foreach (var stud in students)
                {
                    GradesRepository.DeleteGradesBySubject(semester, stud.Id, subject_id);
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ArgumentException();
            }
            finally
            {
                db.Dispose();
            }
        }

        private static int ConvertMonth(int i, int semester)
        {
            int month = 0;
            if (semester == 1)
            {
                if (i == 0) month = 9;
                else if (i == 1) month = 10;
                else if (i == 2) month = 11;
                else if (i == 3) month = 12;
                else if (i == 4) month = 1;
                else if (i == 5) month = 21;
            }
            else if (semester == 2)
            {
                if (i == 0) month = 2;
                else if (i == 1) month = 3;
                else if (i == 2) month = 4;
                else if (i == 3) month = 5;
                else if (i == 4) month = 6;
                else if (i == 5) month = 22;
                else if (i == 6) month = 23;
            }

            return month;
        }
    }
}
