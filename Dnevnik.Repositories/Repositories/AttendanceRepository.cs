using Dnevnik.Data;
using Dnevnik.Repositories.Repositories;
using Dnevnik.ViewModels.Data;
using Dnevnik.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Dnevnik.Repositories.Repositories
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

        public static AttendanceStatsViewModel CalculateClassAttendance(DateTime? dd1, DateTime? dd2, int class_id)
        {
            int month = DateTime.Now.Month - 1;
            int year = DateTime.Now.Year;
            if (month < 1)
            {
                month = 12;
                year--;
            }
            DateTime d1 = new DateTime(year, month, DateTime.Now.Day);
            DateTime d2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
            if (dd1 != null)
            {
                d1 = dd1.Value;
                d2 = dd2.Value;
            }

            var students = StudentsRepository.GetAllStudents(class_id);
            var attendance = AttendanceRepository.GetAttendanceBetweenDates(d1, d2, class_id);
            int[] izvineni = new int[100];
            string[] neizvineni = new string[100];
            int count = 0;
            foreach (var stud in students)
            {
                var studNeiz = attendance.Where(a => a.Student_id == stud.Id && a.Att_type == 0).Count();
                var studZak = attendance.Where(a => a.Student_id == stud.Id && a.Att_type == 1).Count();
                var studIzv = attendance.Where(a => a.Student_id == stud.Id && a.Att_type == 2).Count();

                izvineni[count] = studIzv;

                int c = studZak - studZak % 3;
                studNeiz += c / 3;
                string neizv = studNeiz.ToString();
                if (studZak % 3 != 0)
                {
                    neizv += " " + studZak % 3 + "/3";
                }
                neizvineni[count] = neizv;

                count++;
            }

            AttendanceStatsViewModel vm = new AttendanceStatsViewModel()
            {
                Izvineni = izvineni,
                Neizvineni = neizvineni,
                Students = students,
                Date1 = d1,
                Date2 = d2
            };

            return vm;
        }
    }
}
