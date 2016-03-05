namespace Dnevnik.ViewModels.Data
{
    using Dnevnik.Data;
    using System.Collections.Generic;
    public class SubjectStats
    {
        public SubjectVM Subject { get; set; }
        public int AllStudents { get; set; }
        public int StudentsWithGrades { get; set; }
        public List<Grade> Grades { get; set; }
        public double Average { get; set; }
    }
}
