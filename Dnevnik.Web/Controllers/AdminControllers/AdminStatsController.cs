﻿using Dnevnik.Data;
using Dnevnik.Repositories.Helpers;
using Dnevnik.Repositories.Repositories;
using Dnevnik.ViewModels.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers.AdminControllers
{
    public class AdminStatsController : AdminController
    {
        public ActionResult Show(int class_id = -1)
        {
            if (class_id == -1)
            {
                var classes = DB.GetClasses();
                return View(classes);
            }
            else
            {
                var students = StudentsRepository.GetAllStudents(class_id);
                var subjects = DB.GetClassSubjects(class_id);


                string studClass = students[0].Class.Number.ToString();
                if (students[0].Class.Letter == 1) studClass += "a";
                else if (students[0].Class.Letter == 2) studClass += "б";
                else if (students[0].Class.Letter == 3) studClass += "в";
                else if (students[0].Class.Letter == 4) studClass += "г";
                else if (students[0].Class.Letter == 5) studClass += "д";

                var attendance = AttendanceRepository.CalculateClassAttendance(DateTime.Now.AddYears(-2), DateTime.Now.AddYears(2), class_id);

                var stats1 = StatsHelpers.GetSubjectsStats(21, class_id, false);
                var zipStats1 = StatsHelpers.GetSubjectsStats(21, class_id, true);

                var stats2 = StatsHelpers.GetSubjectsStats(23, class_id, false);
                var zipStats2 = StatsHelpers.GetSubjectsStats(23, class_id, true);

                StatsViewModel vm1 = new StatsViewModel()
                {
                    AllSubjects = stats1,
                    AllZipSubjects = zipStats1,
                    Semester = 1
                };

                StatsViewModel vm2 = new StatsViewModel()
                {
                    AllSubjects = stats2,
                    AllZipSubjects = zipStats2,
                    Semester = 3
                };

                AdminStatsViewModel vm = new AdminStatsViewModel()
                {
                    Students = students,
                    Subjects = subjects,
                    SelectedClass = studClass,
                    Attendance = attendance,
                    Semester1 = vm1,
                    Semester2 = vm2
                };
                return View("ShowClass", vm);
            }
        }


        [HttpGet]
        public FileResult GetStatsToExcel(int semester)
        {
            //TODO: Year report not working... get stats for both semesters and combine them... no idea....
            var file = Dnevnik.Repositories.MSExcel.Write.GetSemesterStats(semester);
            file.SaveAs(Server.MapPath("~/Content/TempFiles/something2.xlsx"));
            file.Close();

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=doklad.xlsx");
            Response.WriteFile(Server.MapPath("~/Content/TempFiles/something2.xlsx"));
            Response.End();
            return null;
        }
    }
}