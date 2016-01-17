using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.Data.AdminRepositories
{
    public static class ClassesRepository
    {
        public static void AddClass(Class c)
        {
            using (var db = new DnevnikEntities())
            {
                db.Classes.Add(c);
                var entry = db.Entry(c);
                entry.State = EntityState.Added;
                db.SaveChanges();
            }
        }

        public static List<Class> GetClasses()
        {
            var db = new DnevnikEntities();
            var classes = db.Classes.Where(c => c.Id > 1)
                .OrderBy(c => c.Number)
                .ThenBy(c => c.Letter).ToList();
            db.Dispose();
            return classes;
        }

        public static void UpdateClasses(List<Class> c)
        {
            using (var db = new DnevnikEntities())
            {
                foreach (var item in c)
                {
                    db.Classes.Add(item);
                    var entry = db.Entry(item);
                    entry.State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
    }
}
