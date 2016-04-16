using Dnevnik.Data;
using Dnevnik.Repositories.AdminRepositories;
using Dnevnik.Repositories.Repositories;
using Dnevnik.Repositories;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dnevnik.Repositories.MSExcel
{
    public static class Write
    {
        private static int x = 7;


        public static Microsoft.Office.Interop.Excel._Workbook GetSemesterStats(int semester = 21)
        {
            var oXL = new Application();
            //oXL.Visible = true;
            var oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add());

            WriteSubjectStats(oXL, semester);

            return oWB;
        }


        private static void WriteSubjectStats(Application oXL, int semester)
        {
            var classes = ClassesRepository.GetClasses();
            classes.Reverse();
            int y = 1;

            int count = 1;
            foreach (var cl in classes)
            {
                Microsoft.Office.Interop.Excel.Worksheet newWorksheet;
                newWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)oXL.Application.Worksheets.Add();
                newWorksheet.Name = cl.Number + DB.ConvertClassLetter(cl.Letter);

                newWorksheet.Range[newWorksheet.Cells[2, 1], newWorksheet.Cells[2, 10]].Merge();
                newWorksheet.Cells[2, 1] = cl.Number + DB.ConvertClassLetter(cl.Letter);


                for (int p = 0; p <= 1; p++)
                {
                    int totalIzpitani = 0;
                    int total2 = 0;
                    int total3 = 0;
                    int total4 = 0;
                    int total5 = 0;
                    int total6 = 0;
                    int totalNeizpitani = 0;
                    double totalAverage = 0;
                    int totalGrades = 0;

                    int tableHeadersX = x - 2;
                    int tableHeadersY = y;
                    var stats = Helpers.StatsHelpers.GetSubjectsStats(semester, cl.Id, Convert.ToBoolean(p));

                    newWorksheet.Range[newWorksheet.Cells[x - 3, 1], newWorksheet.Cells[x - 3, 10]].Merge();
                    if (p == 0)
                    {
                        newWorksheet.Cells[x - 3, tableHeadersY] = "ЗАДЪЛЖИТЕЛНА И ПРОФИЛИРАНА ПОДГОТОВКА";
                    }
                    else
                    {
                        newWorksheet.Cells[x - 3, tableHeadersY] = "ЗАДЪЛЖИТЕЛНОИЗБИРАЕМА ПОДГОТОВКА";
                    }

                    newWorksheet.Range[newWorksheet.Cells[tableHeadersX, tableHeadersY], newWorksheet.Cells[tableHeadersX + 1, tableHeadersY]].Merge();
                    newWorksheet.Cells[tableHeadersX, tableHeadersY] = "№";
                    tableHeadersY++;

                    newWorksheet.Range[newWorksheet.Cells[tableHeadersX, tableHeadersY], newWorksheet.Cells[tableHeadersX + 1, tableHeadersY]].Merge();
                    newWorksheet.Cells[tableHeadersX, tableHeadersY] = "Предмет";
                    tableHeadersY++;

                    newWorksheet.Range[newWorksheet.Cells[tableHeadersX, tableHeadersY], newWorksheet.Cells[tableHeadersX + 1, tableHeadersY]].Merge();
                    newWorksheet.Cells[tableHeadersX, tableHeadersY] = "Изпитани";
                    tableHeadersY++;

                    newWorksheet.Range[newWorksheet.Cells[tableHeadersX, tableHeadersY], newWorksheet.Cells[tableHeadersX, tableHeadersY + 4]].Merge();
                    newWorksheet.Cells[tableHeadersX, tableHeadersY] = "Оценки";

                    newWorksheet.Cells[tableHeadersX + 1, tableHeadersY] = "2";
                    tableHeadersY++;
                    newWorksheet.Cells[tableHeadersX + 1, tableHeadersY] = "3";
                    tableHeadersY++;
                    newWorksheet.Cells[tableHeadersX + 1, tableHeadersY] = "4";
                    tableHeadersY++;
                    newWorksheet.Cells[tableHeadersX + 1, tableHeadersY] = "5";
                    tableHeadersY++;
                    newWorksheet.Cells[tableHeadersX + 1, tableHeadersY] = "6";
                    tableHeadersY++;

                    newWorksheet.Range[newWorksheet.Cells[tableHeadersX, tableHeadersY], newWorksheet.Cells[tableHeadersX + 1, tableHeadersY]].Merge();
                    newWorksheet.Cells[tableHeadersX, tableHeadersY] = "Неизпитани";
                    tableHeadersY++;

                    newWorksheet.Range[newWorksheet.Cells[tableHeadersX, tableHeadersY], newWorksheet.Cells[tableHeadersX + 1, tableHeadersY]].Merge();
                    newWorksheet.Cells[tableHeadersX, tableHeadersY] = "Среден успех";
                    tableHeadersY++;

                    newWorksheet.Range[newWorksheet.Cells[tableHeadersX-1, 1], newWorksheet.Cells[tableHeadersX+1, 10]].Font.Bold = true;


                    

                    foreach (var subject in stats)
                    {
                        newWorksheet.Cells[x, y] = count;
                        y++;
                        newWorksheet.Cells[x, y] = subject.Subject.Title;
                        y++;
                        newWorksheet.Cells[x, y] = subject.StudentsWithGrades;
                        totalIzpitani += subject.StudentsWithGrades;
                        y++;

                        for (int i = 2; i <= 6; i++)
                        {
                            newWorksheet.Cells[x, y] = subject.Grades.Where(g => g.Grade1 == i).Count();
                            y++;
                        }

                        int studentsWithoutGrades = subject.AllStudents - subject.StudentsWithGrades;
                        newWorksheet.Cells[x, y] = studentsWithoutGrades;
                        totalNeizpitani += studentsWithoutGrades;
                        y++;
                        newWorksheet.Cells[x, y] = Math.Round(subject.Average, 2);
                        if (subject.Average != 0)
                        {
                            totalAverage += subject.Average;
                            totalGrades++;
                        }

                        x++;
                        y = 1;
                        count++;
                    }

                    newWorksheet.Cells[x, 2] = "Всичко";
                    newWorksheet.Cells[x, 3] = totalIzpitani;
                    newWorksheet.Cells[x, 4] = total2;
                    newWorksheet.Cells[x, 5] = total3;
                    newWorksheet.Cells[x, 6] = total4;
                    newWorksheet.Cells[x, 7] = total5;
                    newWorksheet.Cells[x, 8] = total6;
                    newWorksheet.Cells[x, 9] = totalNeizpitani;
                    newWorksheet.Cells[x, 10] = Math.Round(totalAverage / totalGrades, 2);

                    newWorksheet.Range[newWorksheet.Cells[tableHeadersX - 1, 1], newWorksheet.Cells[x, 10]].Borders.LineStyle 
                        = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    count = 1;
                    x += 5;
                    y = 1;
                }

                x -= 5;
                WriteAttendanceStats(newWorksheet, semester, cl.Id);

                if (cl.Teachers.Count() != 0)
                {
                    x += 2;
                    newWorksheet.Cells[x, 1] = "Класен ръководител";
                    newWorksheet.Cells[x, 2] = cl.Teachers.FirstOrDefault().Name;
                    newWorksheet.Range[newWorksheet.Cells[x, 1], newWorksheet.Cells[x, 2]].Font.Bold = true;
                    newWorksheet.Range[newWorksheet.Cells[x, 1], newWorksheet.Cells[x, 2]].Borders.LineStyle
                        = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                }

                newWorksheet.get_Range("A1", "Q200").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                newWorksheet.get_Range("A1", "Q200").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                newWorksheet.get_Range("A1", "Q200").Columns.AutoFit();
                newWorksheet.get_Range("A1", "Q200").Rows.AutoFit();

                x = 7;
                y = 1;
                count = 1;
            }

        }

        private static void WriteAttendanceStats(_Worksheet newWorksheet, int semester, int class_id)
        {
            var students = StudentsRepository.GetAllStudents(class_id);

            int nothing = 0;
            int neizvineni = 0;
            List<Student> studentsNeizv = new List<Student>();
            if (semester % 20 == 1)
            {
                nothing = students.Where(s => s.Attendances.Where(a => a.Date1.Month >= 2 && a.Date1.Month <= 6).Count() == 0).Count();
                studentsNeizv = students.Where(s =>
                    s.Attendances
                    .Where(a =>
                        (a.Att_type == 0 || a.Att_type == 1) && (a.Date1.Month >= 2 && a.Date1.Month <= 6))
                        .Count() > 0).ToList();
                neizvineni = studentsNeizv.Count;
            }
            else
            {
                nothing = students.Where(s => s.Attendances.Where(a => a.Date1.Month >= 9 || a.Date1.Month == 1).Count() == 0).Count();
                studentsNeizv = students.Where(s =>
                    s.Attendances
                    .Where(a =>
                        (a.Att_type == 0 || a.Att_type == 1) && (a.Date1.Month >= 9 || a.Date1.Month == 1))
                        .Count() > 0).ToList();
                neizvineni = studentsNeizv.Count;
            }


            x += 2;
            int header = x;
            newWorksheet.Range[newWorksheet.Cells[x, 1], newWorksheet.Cells[x, 2]].Merge();
            newWorksheet.Cells[x, 1] = "Отсъствия";
            newWorksheet.Range[newWorksheet.Cells[x, 1], newWorksheet.Cells[x, 2]].Font.Bold = true;
            x++;

            newWorksheet.Cells[x, 1] = "Без отсъствия";
            newWorksheet.Cells[x, 2] = nothing.ToString();
            x++;
            newWorksheet.Cells[x, 1] = "С неизвинени отсъствия";
            newWorksheet.Cells[x, 2] = neizvineni.ToString();
            x++;

            newWorksheet.Cells[x, 1] = "До 3 часа";
            newWorksheet.Cells[x, 2] = studentsNeizv
                .Where(s =>
                    s.Attendances.Where(a => a.Att_type == 0).Count() + (s.Attendances.Where(a => a.Att_type == 1).Count() / 3) <= 3).Count();

            x++;

            newWorksheet.Cells[x, 1] = "До 10 часа";
            newWorksheet.Cells[x, 2] = studentsNeizv
                .Where(s =>
                    s.Attendances.Where(a => a.Att_type == 0).Count() + (s.Attendances.Where(a => a.Att_type == 1).Count() / 3) > 3 &&
                    s.Attendances.Where(a => a.Att_type == 0).Count() + (s.Attendances.Where(a => a.Att_type == 1).Count() / 3) <= 10).Count();

            x++;

            newWorksheet.Cells[x, 1] = "До 15 часа";
            newWorksheet.Cells[x, 2] = studentsNeizv
                .Where(s =>
                    s.Attendances.Where(a => a.Att_type == 0).Count() + (s.Attendances.Where(a => a.Att_type == 1).Count() / 3) > 10 &&
                    s.Attendances.Where(a => a.Att_type == 0).Count() + (s.Attendances.Where(a => a.Att_type == 1).Count() / 3) <= 15).Count();

            x++;

            newWorksheet.Cells[x, 1] = "До 25 часа";
            newWorksheet.Cells[x, 2] = studentsNeizv
                .Where(s =>
                    s.Attendances.Where(a => a.Att_type == 0).Count() + (s.Attendances.Where(a => a.Att_type == 1).Count() / 3) > 15 &&
                    s.Attendances.Where(a => a.Att_type == 0).Count() + (s.Attendances.Where(a => a.Att_type == 1).Count() / 3) <= 25).Count();

            x++;

            newWorksheet.Cells[x, 1] = "До 10 часа";
            newWorksheet.Cells[x, 2] = studentsNeizv
                .Where(s =>
                    s.Attendances.Where(a => a.Att_type == 0).Count() + (s.Attendances.Where(a => a.Att_type == 1).Count() / 3) > 25).Count();


            newWorksheet.Range[newWorksheet.Cells[header, 1], newWorksheet.Cells[x, 2]].Borders.LineStyle
                        = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            //var att = AttendanceRepository.GetAttendanceBetweenDates(DateTime.Now.AddYears(-2), DateTime.Now.AddYears(2), class_id);

            //if (semester == 2) att = att.Where(a => a.Date1.Month >= 2 && a.Date1.Month <= 6).ToList();
            //else att = att.Where(a => a.Date1.Month >= 9 || a.Date1.Month == 1).ToList();


        }
    }
}
