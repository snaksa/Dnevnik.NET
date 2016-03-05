using Dnevnik.ViewModels.Data;
using System.Collections.Generic;

namespace Dnevnik.ViewModels.Web
{
    public class GradesViewModel
    {
        public List<StudentGradesViewModel> Students { get; set; }
        public int? Subject_id { get; set; }
        public int Semester { get; set; }
    }
}