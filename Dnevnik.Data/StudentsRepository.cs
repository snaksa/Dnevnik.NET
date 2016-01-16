using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.Data
{
    public static class StudentsRepository
    {
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
    }
}
