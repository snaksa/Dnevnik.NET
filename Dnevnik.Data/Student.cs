//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dnevnik.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        public Student()
        {
            this.Grades = new HashSet<Grade>();
            this.Attendances = new HashSet<Attendance>();
        }
    
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int Class_id { get; set; }
        public string EGN { get; set; }
    
        public virtual Class Class { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
