using Dnevnik.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dnevnik.Web.ViewModels
{
    public class AttendanceViewModel
    {
        public string Date { get; set; }
        public DateTime Date2 { get; set; }
        public List<PeriodAttendance> Periods { get; set; }
    }
}