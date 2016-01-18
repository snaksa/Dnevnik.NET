using System.Collections.Generic;

namespace Dnevnik.Data.ViewModels
{
    public class PeriodAttendance
    {
        public int PeriodId { get; set; }
        public string Izvineni { get; set; }
        public string Neizvineni { get; set; }
        public string Zakusneniq { get; set; }
    }
}
