using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.ViewModels.Data
{
    public class StudentGradesViewModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public List<StudentGrade> GradesArray { get; set; }
        public string[] Grades { get; set; }
    }
}
