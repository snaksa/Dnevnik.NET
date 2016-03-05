using System;
using Dnevnik.Data;
using Dnevnik.ViewModels.Data;
using System.Collections.Generic;

namespace Dnevnik.ViewModels.Web
{
    public class AttendanceViewModel
    {
        public string Date { get; set; }
        public DateTime Date2 { get; set; }
        public List<Attendance> AllAttendances { get; set; }
        public List<Schedule> AllSchedule { get; set; }
        public PeriodAttendance[] Periods { get; set; }

    }
}