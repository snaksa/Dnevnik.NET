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
        public static int LoginUser(string username, string password)
        {
            var db = new DnevnikEntities();
            var user = db.Teachers.Where(t => t.Email == username && t.Password == password).FirstOrDefault();
            if (user == null) return -1;
            else return user.Id;
        }

        public static Teacher GetCurrentUser(int id)
        {
            var dbContext = new DnevnikEntities();
            var teacher = dbContext.Teachers.Where(t => t.Id == id).FirstOrDefault();
            dbContext.Dispose();
            return teacher;
        }

        public static List<Subject> GetSubjects()
        {
            var db = new DnevnikEntities();
            var subjects = db.Subjects.ToList();
            db.Dispose();
            return subjects;
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
    }
}
