using Dnevnik.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Dnevnik.Repositories.AdminRepositories
{
    public static class TeachersRepository
    {
        public static void AddTeacher(Teacher t)
        {
            t.Class_id = 1;
            var db = new DnevnikEntities();
            db.Teachers.Add(t);
            var entry = db.Entry(t);
            entry.State = EntityState.Added;
            db.SaveChanges();
            db.Dispose();
        }

        public static List<Teacher> GetTeachers()
        {
            var db = new DnevnikEntities();
            var teachers = db.Teachers.Where(t => t.IsAdmin == 0).ToList();
            db.Dispose();
            return teachers;
        }

        public static void UpdateTeachers(List<Teacher> teachers)
        {
            using (var db = new DnevnikEntities())
            {
                foreach (var item in teachers)
                {
                    db.Teachers.Add(item);
                    var entry = db.Entry(item);
                    entry.State = EntityState.Modified;
                    entry.Property(e => e.IsAdmin).IsModified = false;
                }
                db.SaveChanges();
            }
        }

        public static void DeleteTeacher(int id)
        {
            using (var db = new DnevnikEntities())
            {
                db.Teachers.Remove(db.Teachers.Where(t => t.Id == id).FirstOrDefault());
                db.SaveChanges();
            }
        }
    }
}
