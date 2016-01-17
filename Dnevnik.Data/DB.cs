﻿namespace Dnevnik.Data
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
        public static Teacher LoginUser(string username, string password)
        {
            var db = new DnevnikEntities();
            var user = db.Teachers.Where(t => t.Email == username && t.Password == password).FirstOrDefault();
            return user;
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

        private static string ConvertClassLetter(int letter)
        {
            if (letter == 1) return "а";
            if (letter == 2) return "б";
            if (letter == 3) return "в";
            if (letter == 4) return "г";
            if (letter == 5) return "д";
            return "е";
        }

        public static List<SingleClass> GetClasses()
        {
            var db = new DnevnikEntities();
            var classes = db.Classes.Where(c => c.Id > 1).OrderBy(c => c.Number).ThenBy(c => c.Letter);

            List<SingleClass> classesList = new List<SingleClass>();
            foreach (var item in classes)
            {
                SingleClass c = new SingleClass()
                {
                    Id = item.Id,
                    Number = item.Number,
                    Letter = ConvertClassLetter(item.Letter)
                };
                classesList.Add(c);
            }

            db.Dispose();
            return classesList;
        }

        public static void UpdateTeacherSettings(Teacher t)
        {
            using (var db = new DnevnikEntities())
            {
                db.Teachers.Add(t);
                var entry = db.Entry(t);
                entry.State = EntityState.Modified;
                entry.Property(e => e.IsAdmin).IsModified = false;
                entry.Property(e => e.Class_id).IsModified = false;
                db.SaveChanges();
            }
        }
    }
}
