using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.ViewModels.Data
{
    public class SubjectVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Semester { get; set; }
        public bool IsZip { get; set; }
    }
}
