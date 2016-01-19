using Dnevnik.Data;
using Dnevnik.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;

namespace Dnevnik.Web.ViewModels
{
    public class IndexViewModel
    {
        public Student CurrentStudent { get; set; }
        public List<SubjectVM> Subjects { get; set; }
    }
}