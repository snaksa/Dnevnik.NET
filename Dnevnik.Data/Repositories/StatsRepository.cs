using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.Data.Repositories
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
            var grades = db.Grades.Where(g => g.Grade_month == semester && g.Student.Class_id == class_id).ToList();
            db.Dispose();
            return grades;
        }

    }
}
