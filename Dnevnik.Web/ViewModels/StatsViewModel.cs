namespace Dnevnik.Web.ViewModels
{
    using Dnevnik.Data.ViewModels;
using System.Collections.Generic;
    public class StatsViewModel
    {
        public List<SubjectStats> AllSubjects { get; set; }
        public List<SubjectStats> AllZipSubjects { get; set; }
        public int Semester { get; set; }
    }
}