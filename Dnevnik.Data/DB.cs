namespace Dnevnik.Data
{
    using Dnevnik.Data.ViewModels;
    using Dnevnik.Web.Models;
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

        public static List<StudentGradesViewModel> GetFirstSemesterGrades(int class_id, int? subject_id)
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
                    GradesArray = s.Grades.Where(g => g.Subject_id == subject_id).Select(g => new StudentGrade { 
                        Month = g.Grade_month,
                        Grade = g.Grade1
                    }).ToList()
                })
                .ToList();
            return grades;
        }

    }
}
