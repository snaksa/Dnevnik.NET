using System;
using Dnevnik.Data;
using Dnevnik.ViewModels.Data;
using System.Collections.Generic;

namespace Dnevnik.ViewModels.Web
{
    public class IndexViewModel
    {
        public Student CurrentStudent { get; set; }
        public List<SubjectVM> Subjects { get; set; }
        public List<Schedule> Schedule1 { get; set; }
        public List<Schedule> Schedule2 { get; set; }
    }
}