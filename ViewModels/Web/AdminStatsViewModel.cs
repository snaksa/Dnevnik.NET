using Dnevnik.Data;
using Dnevnik.ViewModels.Data;
using System.Collections.Generic;

namespace Dnevnik.ViewModels.Web
{
    public class AdminStatsViewModel
    {
        public string SelectedClass { get; set; }
        public List<Student> Students { get; set; }
        public List<SubjectVM> Subjects { get; set; }
    }
}