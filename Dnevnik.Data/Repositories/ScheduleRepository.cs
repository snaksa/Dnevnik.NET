using Dnevnik.Data.ViewModels;
using Dnevnik.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.Data
{
    public static class ScheduleRepository
    {
        public static List<Schedule> GetSchedule(int semester, int class_id)
        {
            var db = new DnevnikEntities();
            var schedule = db.Schedules
                .Include("Subject")
                .Where(s => s.Class_id == class_id && s.Semester == semester)
                .OrderBy(s => s.Day)
                .ThenBy(s => s.Period)
                .ToList();
            db.Dispose();
            return schedule;
        }

        public static List<SubjectVM> GetAllSchedule(int class_id)
        {
            var db = new DnevnikEntities();
            var schedule = db.Schedules
                .Include("Subject")
                .Where(s => s.Class_id == class_id)
                .GroupBy(x => x.Subject_id)
                .Select(y => new SubjectVM { 
                    Id = y.Key,
                    Title = y.FirstOrDefault().Subject.Title
                })
                .ToList();


            db.Dispose();

            return schedule;
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
    }
}
