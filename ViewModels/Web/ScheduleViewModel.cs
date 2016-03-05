using Dnevnik.Data;
using System;
using System.Collections.Generic;

namespace Dnevnik.ViewModels.Web
{
    public class ScheduleViewModel
    {
        public int[,] ScheduleList { get; set; }
        public int[] Periods { get; set; }
        public List<Dnevnik.Data.Subject> Items { get; set; }
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