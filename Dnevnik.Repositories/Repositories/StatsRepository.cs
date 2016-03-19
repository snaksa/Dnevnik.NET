using Dnevnik.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.Repositories.Repositories
{
    public static class StatsRepository
    {
        //private static 

        public static int GetAllStudentsCount(int class_id)
        {
            DnevnikEntities db = new DnevnikEntities();
            var students = db.Students.Where(s => s.Class_id == class_id).Count();
            db.Dispose();
            return students;
        }

        public static List<Grade> GetAllGrades(int class_id, int semester)
        {
            DnevnikEntities db = new DnevnikEntities();
            var grades = new List<Grade>();
            if (semester == 1) 
                grades = db.Grades.Where(g => ((g.Grade_month >= 9 && g.Grade_month <= 12) || g.Grade_month == 1) && g.Student.Class_id == class_id).ToList();
            else if(semester == 2)
                grades = db.Grades.Where(g => (g.Grade_month >= 2 && g.Grade_month <= 6) && g.Student.Class_id == class_id).ToList();
            else
                grades = db.Grades.Where(g => g.Grade_month == semester && g.Student.Class_id == class_id).ToList();
            
            db.Dispose();
            return grades;
        }

    }
}
