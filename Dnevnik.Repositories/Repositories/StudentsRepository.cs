using Dnevnik.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.Repositories.Repositories
{
    public static class StudentsRepository
    {
        public static Student GetStudent(string egn)
        {
            var dbContext = new DnevnikEntities();
            var student = dbContext
                .Students
                .Include("Grades")
                .Include("Attendances")
                .Where(s => s.EGN == egn)
                .FirstOrDefault();
            dbContext.Dispose();
            return student;
        }

        public static List<Student> GetAllStudents(int class_id)
        {
            var db = new DnevnikEntities();
            var students = db.Students.Include("Grades").Include("Class").Where(s => s.Class_id == class_id).OrderBy(s => s.Number).ToList();
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
                entry.Property(e => e.EGN).IsModified = true;
            }

            db.SaveChanges();
            db.Dispose();
        }

        public static void AddStudent(Student student, int class_id)
        {
            Student stud = new Student()
            {
                EGN = student.EGN,
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

        public static void DeleteStudent(int id, int class_id)
        {
            var db = new DnevnikEntities();
            var student = db.Students.Where(s => s.Id == id && s.Class_id == class_id).FirstOrDefault();
            if (student == null) throw new ArgumentException();

            db.Grades.RemoveRange(db.Grades.Where(g => g.Student_id == id));
            db.Attendances.RemoveRange(db.Attendances.Where(a => a.Student_id == id).ToList());
            db.Students.Remove(student);
            //remove attendance records

            db.SaveChanges();
            db.Dispose();
        }

        public static void ImportStudents(List<Student> students)
        {
            var db = new DnevnikEntities();
            foreach (var stud in students)
            {
                db.Students.Add(stud);
                db.Entry(stud).State = EntityState.Added;
            }
            db.SaveChanges();
            db.Dispose();
        }

        public static void DeleteAllStudents()
        {
            GradesRepository.DeleteAllGrades();
            AttendanceRepository.RemoveAllAttendance();

            var db = new DnevnikEntities();
            db.Students.RemoveRange(db.Students);
            db.SaveChanges();
            db.Dispose();
        }
    }
}
