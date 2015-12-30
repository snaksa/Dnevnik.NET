namespace Dnevnik.Web.ViewModels
{
    using Dnevnik.Data;
    using System.Collections.Generic;

    public class StudentsViewModel
    {
        public Student StudentToAdd { get; set; }
        public List<Student> AllStudents { get; set; }
    }
}