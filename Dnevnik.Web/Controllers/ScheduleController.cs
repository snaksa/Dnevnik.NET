using Dnevnik.Data;
using Dnevnik.Data.ViewModels;
using Dnevnik.Web.Models;
using Dnevnik.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers
{
    public class ScheduleController : BaseController
    {
        [HttpGet]
        public ActionResult FirstSemester()
        {
            return Show(1);
        }

        [HttpGet]
        public ActionResult SecondSemester()
        {
            return Show(2);
        }

        [HttpGet]
        public ActionResult Show(int semester)
        {
            TempData["semester"] = semester;

            var subjects = DB.GetSubjects();
            var schedule = ScheduleRepository.GetSchedule(semester, this.CurrentUser.Class_id);

            int[,] sch = new int[6, 8];
            foreach (var period in schedule)
                sch[period.Day, period.Period] = period.Subject.Id;


            int[] periods = new int[35];
            ScheduleViewModel vm = new ScheduleViewModel()
            {
                ScheduleList = sch,
                Items = subjects,
                Periods = periods,
                Semester = semester
            };

            return View("Show", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSchedule(ScheduleViewModel vm)
        {
            int class_id = this.CurrentUser.Class_id;
            ScheduleRepository.DeleteOldSchedule(class_id, vm.Semester);


            List<Schedule> periods = new List<Schedule>();
            for (int i = 0; i < 35; i++)
            {
                if (vm.Periods[i] != 1)
                {
                    double a = (i + 1) / 7.0;
                    double day = Math.Ceiling(a);
                    double period = 7 - ((Math.Ceiling(a) * 7) - (i + 1));
                    Schedule p = new Schedule
                    {
                        Day = (int)day,
                        Period = (int)period,
                        Class_id = class_id,
                        Subject_id = vm.Periods[i],
                        Semester = vm.Semester
                    };
                    periods.Add(p);
                }
            }

            try
            {
                ScheduleRepository.AddNewSchedule(periods);
                TempData["success"] = "1";
            }
            catch (Exception ex)
            {
                TempData["success"] = "0";
            }

            //success message

            if (vm.Semester == 1) return RedirectToAction("FirstSemester");
            else return RedirectToAction("SecondSemester");
        }
    }
}