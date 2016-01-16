using System.Collections.Generic;

namespace Dnevnik.Data.ViewModels
{
    public class PeriodAttendance
    {
        public int Period { get; set; }
        public string Title { get; set; }
        public List<SingleAttendance> Attendance { get; set; }

        public string Izvineni { get; set; }
        public string Neizvineni { get; set; }
        public string Zakusneniq { get; set; }
    }
}
