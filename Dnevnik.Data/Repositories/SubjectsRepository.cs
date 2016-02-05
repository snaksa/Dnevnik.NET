using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.Data.Repositories
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

        public static void AddSubject(string title)
        {
            using (var db = new DnevnikEntities())
            {
                Subject s = new Subject()
                {
                    Title = title
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
    }
}
