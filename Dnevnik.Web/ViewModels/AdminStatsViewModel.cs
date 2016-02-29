using Dnevnik.Data;
using Dnevnik.Data.ViewModels;
using System.Collections.Generic;
namespace Dnevnik.Web.ViewModels
{
    public class AdminStatsViewModel
    {
        public string SelectedClass { get; set; }
        public List<Student> Students { get; set; }
        public List<SubjectVM> Subjects { get; set; }
    }
}