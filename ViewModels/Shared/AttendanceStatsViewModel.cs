using Dnevnik.Data;
using System;
using System.Collections.Generic;

namespace Dnevnik.ViewModels.Shared
{
    public class AttendanceStatsViewModel
    {
        public List<Student> Students { get; set; }
        public int[] Izvineni { get; set; }
        public string[] Neizvineni { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
    }
}