using Dnevnik.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dnevnik.Web.ViewModels
{
    public class GradesViewModel
    {
        public List<StudentGradesViewModel> Students { get; set; }
        public int? Subject_id { get; set; }
        public int Semester { get; set; }
    }
}