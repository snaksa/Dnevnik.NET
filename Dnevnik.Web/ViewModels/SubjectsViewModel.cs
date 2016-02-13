using Dnevnik.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dnevnik.Web.ViewModels
{
    public class SubjectsViewModel
    {
        public List<Subject> Subjects { get; set; }

        public string Title { get; set; }
        public bool IsZip { get; set; }
    }
}