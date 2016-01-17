using Dnevnik.Data;
using Dnevnik.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dnevnik.Web.ViewModels
{
    public class TeachersViewModel
    {
        public Teacher TeacherToAdd { get; set; }
        public List<Teacher> AllTeachers { get; set; }
        public List<SingleClass> AllClasses { get; set; }
    }
}