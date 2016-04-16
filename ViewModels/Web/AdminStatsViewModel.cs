using Dnevnik.Data;
using Dnevnik.ViewModels.Data;
using Dnevnik.ViewModels.Shared;
using System.Collections.Generic;

namespace Dnevnik.ViewModels.Web
{
    public class AdminStatsViewModel
    {
        public string SelectedClass { get; set; }
        public List<Student> Students { get; set; }
        public List<SubjectVM> Subjects { get; set; }
        public AttendanceStatsViewModel Attendance { get; set; }

        public StatsViewModel Semester1 { get; set; }
        public StatsViewModel Semester2 { get; set; }
    }
}