using Dnevnik.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.ViewModels
{
    public class ScheduleViewModel
    {
        public int[,] ScheduleList { get; set; }
        public int[] Periods { get; set; }
        public List<Subject> Items { get; set; }
        public int Semester { get; set; }

        public string[] Days = new string[6] {
            "",
            "Понеделник",
            "Вторник",
            "Сряда",
            "Четвъртък",
            "Петък"
        };

    }
}