using Dnevnik.Data;
using Dnevnik.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dnevnik.ViewModels.Web
{
    public class TeachersViewModel
    {
        public Teacher TeacherToAdd { get; set; }
        public List<Teacher> AllTeachers { get; set; }
        public List<SingleClass> AllClasses { get; set; }
    }
}