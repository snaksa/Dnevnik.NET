namespace Dnevnik.ViewModels.Web
{
    using Dnevnik.ViewModels.Data;
    using System.Collections.Generic;
    public class StatsViewModel
    {
        public List<SubjectStats> AllSubjects { get; set; }
        public List<SubjectStats> AllZipSubjects { get; set; }
        public int Semester { get; set; }
    }
}