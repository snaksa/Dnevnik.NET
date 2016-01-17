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
