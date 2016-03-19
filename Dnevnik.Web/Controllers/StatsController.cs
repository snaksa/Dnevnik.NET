using Dnevnik.Data;
using Dnevnik.Repositories.Repositories;
using Dnevnik.ViewModels.Data;
using Dnevnik.ViewModels.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dnevnik.Repositories.Helpers;
using System.IO;

namespace Dnevnik.Web.Controllers
{
    public class StatsController : BaseController
    {
        // GET: Stats
        public ActionResult Show(int semester = 21)
        {
            var stats = StatsHelpers.GetSubjectsStats(semester, this.CurrentUser.Class_id, false);
            var zipStats = StatsHelpers.GetSubjectsStats(semester, this.CurrentUser.Class_id, true);

            StatsViewModel vm = new StatsViewModel()
            {
                AllSubjects = stats,
                AllZipSubjects = zipStats,
                Semester = semester % 20
            };

            return View(vm);
        }

        public ActionResult Attendance(DateTime? dd1, DateTime? dd2)
        {
            var vm = AttendanceRepository.CalculateClassAttendance(dd1, dd2, this.CurrentUser.Class_id);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Attendance(string dd1, string dd2)
        {
            string[] date = dd1.Split('-');
            DateTime d1 = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]));

            date = dd2.Split('-');
            DateTime d2 = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]));

            return RedirectToAction("Attendance", new { dd1 = d1, dd2 = d2 });
        }
    }
}