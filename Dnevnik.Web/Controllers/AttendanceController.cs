using Dnevnik.Data;
using Dnevnik.Data.Repositories;
using Dnevnik.Data.ViewModels;
using Dnevnik.Web.Filters;
using Dnevnik.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers
{
    public class AttendanceController : BaseController
    {
        [HttpGet]
        public ActionResult Show(DateTime? d)
        {
            DateTime date = DateTime.Today;
            if (d != null) date = d.Value;
            string dateStr = date.ToString("dd-MM-yyyy");
            var attendance = AttendanceRepository.GetAttendance(date, this.CurrentUser.Class_id);

            int day = (int)date.DayOfWeek;
            List<Schedule> schedule = new List<Schedule>();

            if(date.Month >= 2 && date.Month <= 6) 
                schedule = ScheduleRepository.GetSchedule(2, this.CurrentUser.Class_id).Where(s => s.Day == day).ToList();
            else if ((date.Month >= 9 && date.Month <= 12) || date.Month == 1) schedule = ScheduleRepository.GetSchedule(1, this.CurrentUser.Class_id).Where(s => s.Day == day).ToList();

            AttendanceViewModel vm = new AttendanceViewModel()
            {
                AllAttendances = attendance,
                AllSchedule = schedule,
                Date = dateStr,
                Date2 = date,
                Periods = new PeriodAttendance[7]
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowAttendance(string dateToShow)
        {
            if (dateToShow == String.Empty) return RedirectToAction("Show");

            string[] date = dateToShow.Split('-');
            DateTime d1 = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]));

            return RedirectToAction("Show", new { d = d1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAttendance(AttendanceViewModel vm)
        {
            try
            {
                AttendanceRepository.UpdateAttendance(vm.Periods, vm.Date2, this.CurrentUser.Class_id);
                TempData["success"] = "1";
            }
            catch (Exception ex)
            {
                TempData["success"] = "0";
            }
            return RedirectToAction("Show", new { d = vm.Date2 });
        }
    }
}