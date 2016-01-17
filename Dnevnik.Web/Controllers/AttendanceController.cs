using Dnevnik.Data;
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
            return View();
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
        public ActionResult UpdateAttendance()
        {
            return null;
        }
    }
}