using Dnevnik.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers.AdminControllers
{
    public class AdminClearDataController : AdminController
    {
        public ActionResult ClearData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteData()
        {
            //delete data
            DB.DeleteAllAttendance();
            DB.DeleteAllGrades();
            DB.DeleteAllSchedule();
            DB.DeleteAllStudents();
            return RedirectToAction("Show", "Teachers");
        }
    }
}