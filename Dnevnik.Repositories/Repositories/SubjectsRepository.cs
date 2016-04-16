using Dnevnik.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.Repositories.Repositories
{
    public static class SubjectsRepository
    {
        public static List<Subject> GetAllSubjects()
        {
            var db = new DnevnikEntities();
            var subjects = db.Subjects.Where(s => s.Id > 1).ToList();
            db.Dispose();
            return subjects;
        }

        public static List<Subject> GetSubjectsByClassAndSemester(int class_id, int semester)
        {
            var db = new DnevnikEntities();
            var sb = db.Schedules.Where(s => s.Class_id == class_id && s.Semester == semester).Select(s => s.Subject).ToList();

            List<Subject> subjects = new List<Subject>();
            List<int> ids = new List<int>();
            foreach (var item in sb)
            {
                if (!ids.Contains(item.Id))
                {
                    ids.Add(item.Id);
                    subjects.Add(item);
                }
            }

            db.Dispose();
            return subjects;
        }

        public static void AddSubject(string title, bool isZip)
        {
            using (var db = new DnevnikEntities())
            {
                Subject s = new Subject()
                {
                    Title = title,
                    IsZip = isZip
                };

                db.Subjects.Add(s);
                var entry = db.Entry(s);
                entry.State = EntityState.Added;
                db.SaveChanges();
            }
        }

        public static void UpdateSubjects(List<Subject> subjects)
        {
            using (var db = new DnevnikEntities())
            {
                foreach (var sub in subjects)
                {
                    if (sub.Title != null && sub.Title != String.Empty)
                    {
                        db.Subjects.Add(sub);
                        var entry = db.Entry(sub);
                        entry.State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }
        }

        public static void DeleteSubject(int id)
        {
            using (var db = new DnevnikEntities())
            {
                db.Schedules.RemoveRange(db.Schedules.Where(s => s.Subject_id == id));
                db.Grades.RemoveRange(db.Grades.Where(g => g.Subject_id == id));
                db.Subjects.Remove(db.Subjects.Where(s => s.Id == id).FirstOrDefault());
                db.SaveChanges();
            }
        }
    }
}
