namespace Dnevnik.Data
{
    using Dnevnik.Data.ViewModels;
    using Dnevnik.Web.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;

    public static class DB
    {
        public static Teacher GetCurrentUser(int id)
        {
            var dbContext = new DnevnikEntities();
            var teacher = dbContext.Teachers.Where(t => t.Id == id).FirstOrDefault();
            dbContext.Dispose();
            return teacher;
        }

        public static List<Student> GetAllStudents(int class_id)
        {
            var db = new DnevnikEntities();
            var students = db.Students.Where(s => s.Class_id == class_id).OrderBy(s => s.Number).ToList();
            db.Dispose();
            return students;
        }

        public static void SaveStudents(List<Student> students)
        {
            var db = new DnevnikEntities();
            foreach (var s in students)
            {
                db.Students.Attach(s);
                var entry = db.Entry(s);
                entry.Property(e => e.Number).IsModified = true;
                entry.Property(e => e.Name).IsModified = true;
                entry.Property(e => e.Note).IsModified = true;
            }

            db.SaveChanges();
            db.Dispose();
        }

        public static void AddStudent(Student student, int class_id)
        {
            Student stud = new Student()
            {
                Number = student.Number,
                Name = student.Name,
                Class_id = class_id
            };

            var db = new DnevnikEntities();
            db.Students.Add(stud);
            db.Entry(stud).State = EntityState.Added;
            db.SaveChanges();
            db.Dispose();
        }

        public static List<ScheduleDay> GetSchedule(int semester, int class_id)
        {
            var db = new DnevnikEntities();
            var schedule = db.Schedules
                .Where(s => s.Class_id == class_id && s.Semester == semester)
                .OrderBy(s => s.Day)
                .ThenBy(s => s.Period)
                .Select(s => new ScheduleDay
                {
                    Day = s.Day,
                    Period = s.Period,
                    Subject = s.Subject
                })
                .ToList();
            db.Dispose();
            return schedule;
        }

        public static List<Subject> GetSubjects()
        {
            var db = new DnevnikEntities();
            var subjects = db.Subjects.ToList();
            db.Dispose();
            return subjects;
        }

        public static void DeleteOldSchedule(int class_id, int semester)
        {
            using (var db = new DnevnikEntities())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM Schedule WHERE Class_id = @id AND Semester = @semester", new SqlParameter("@id", class_id), new SqlParameter("@semester", semester));
            }
        }

        public static void AddNewSchedule(List<Schedule> periods)
        {
            var db = new DnevnikEntities();
            foreach (var p in periods)
            {
                db.Schedules.Add(p);
                db.Entry(p).State = EntityState.Added;
            }
            db.SaveChanges();
            db.Dispose();
        }

        public static List<SubjectVM> GetClassSubjects(int class_id)
        {
            var db = new DnevnikEntities();
            var subjects = db.Schedules
                .Where(s => s.Class_id == class_id)
                .GroupBy(s => new
                {
                    Id = s.Subject.Id,
                    Title = s.Subject.Title
                })
                .Select(o => new SubjectVM
                {
                    Id = o.Key.Id,
                    Title = o.Key.Title
                })
                .ToList();
            db.Dispose();
            return subjects;
        }

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
                    GradesArray = s.Grades.Where(g => g.Subject_id == subject_id).Select(g => new StudentGrade
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

        public static void AddGradesToDB(List<StudentGradesViewModel> students, int semester, int subject_id)
        {
            var db = new DnevnikEntities();

            try
            {
                int len = 0;
                if (semester == 1) len = 6;
                else len = 7;

                foreach (var s in students)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (s.Grades[i] != string.Empty)
                        {
                            var grades = s.Grades[i].Split(new char[]{',', ';'});//remove empty entries
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


                foreach (var stud in students)
                {
                    DB.DeleteGradesBySubject(semester, stud.Id, subject_id);
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

    }
}
