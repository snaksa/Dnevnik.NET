using Dnevnik.ViewModels.Data;
using System.Collections.Generic;
using Dnevnik.Data;

namespace Dnevnik.ViewModels.Web
{
    public class GradesViewModel
    {
        public List<StudentGradesViewModel> Students { get; set; }
        public int? Subject_id { get; set; }
        public int Semester { get; set; }


        public List<Subject> Semester1 { get; set; }
        public List<Subject> Semester2 { get; set; }
        public List<Subject> AllSubjects { get; set; }
    }
}