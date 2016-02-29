using Dnevnik.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Dnevnik.Data.Repositories
{
    public static class AttendanceRepository
    {
        public static List<Attendance> GetAttendance(DateTime date, int class_id)
        {
            var db = new DnevnikEntities();
            var attendance = db.Attendances
                .Include("Student")
                .Where(a => a.Date1 == date && a.Student.Class_id == class_id)
                .OrderBy(a => a.Period)
                .ThenBy(a => a.Student.Number)
                .ToList();

            db.Dispose();
            return attendance;
        }

        public static void UpdateAttendance(PeriodAttendance[] periods, DateTime date, int class_id)
        {
            using (var db = new DnevnikEntities())
            {
                var students = db.Students.Where(s => s.Class_id == class_id).ToList();
                foreach (var period in periods)
                {
                    string izvineni = period.Izvineni;
                    string[] izvineniArr;
                    if (izvineni != null) izvineniArr = izvineni.Split(new char[] { ',', ';' });
                    else izvineniArr = new string[] { "" };

                    string neizvineni = period.Neizvineni;
                    string[] neizvineniArr;
                    if (neizvineni != null) neizvineniArr = neizvineni.Split(new char[] { ',', ';' });
                    else neizvineniArr = new string[] { "" };

                    string zakusneniq = period.Zakusneniq;
                    string[] zakusneniqArr;
                    if (zakusneniq != null) zakusneniqArr = zakusneniq.Split(new char[] { ',', ';' });
                    else zakusneniqArr = new string[] { "" };

                    for (int i = 0; i < 3; i++)
                    {
                        string[] process = null;

                        if (i == 0) process = neizvineniArr;
                        else if (i == 1) process = zakusneniqArr;
                        else process = izvineniArr;

                        foreach (var item in process)
                        {
                            var num = item.Trim();
                            Student student = null;
                            if (num != String.Empty) student = students.Where(s => s.Number == Int32.Parse(num)).FirstOrDefault();
                            if (student != null)
                            {
                                var att = new Attendance()
                                {
                                    Att_type = i,
                                    Date1 = date,
                                    Period = period.PeriodId,
                                    Student_id = student.Id
                                };
                                db.Attendances.Add(att);
                                db.Entry(att).State = EntityState.Added;
                            }
                        }
                    }
                }
                DeleteOldAttendance(date, class_id);
                db.SaveChanges();
            }
        }

        public static void DeleteOldAttendance(DateTime date, int class_id)
        {
            using (var db = new DnevnikEntities())
            {
                db.Attendances
                    .RemoveRange(db.Attendances
                    .Where(a => a.Student.Class_id == class_id && a.Date1 == date)
                    .ToList());
                db.SaveChanges();
            }
        }

        public static List<Attendance> GetAttendanceBetweenDates(DateTime d1, DateTime d2, int class_id)
        {
            using (var db = new DnevnikEntities())
            {
                var att = db.Attendances.Where(a => a.Date1 >= d1 && a.Date1 <= d2 && a.Student.Class_id == class_id).ToList();
                return att;
            }
        }

        internal static void RemoveAllAttendance()
        {
            var db = new DnevnikEntities();
            db.Attendances.RemoveRange(db.Attendances.ToList());
            db.SaveChanges();
            db.Dispose();
        }
    }
}
