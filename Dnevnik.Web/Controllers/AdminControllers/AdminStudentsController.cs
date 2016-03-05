using Dnevnik.Data;
using Dnevnik.Repositories.Repositories;
using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers.AdminControllers
{
    public class AdminStudentsController : AdminController
    {
        // GET: AdminStudents
        public ActionResult Show()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/TempFiles"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    AddStudentsToDB(path);
                    System.IO.File.Delete(path);
                    TempData["success"] = "1";
                }
            }
            catch (Exception ex)
            {
                TempData["success"] = "0";
            }

            return RedirectToAction("Show");
        }

        private void AddStudentsToDB(string path)
        {
            FileStream stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();

            List<Student> students = new List<Student>();
            int tablesCount = result.Tables.Count;
            for (int i = 0; i < tablesCount; i++)
            {
                string tableName = result.Tables[i].TableName;
                var parsed = ParseClass(tableName);
                int id = DB.GetClassId(parsed[0], parsed[1]);
                if (id == 0)
                {
                    continue;
                }

                int row = 4;
                int col = 1;

                while (true)
                {
                    string number = result.Tables[i].Rows[row][col].ToString();
                    if (number == String.Empty) break;
                    string firstName = result.Tables[i].Rows[row][col + 1].ToString();
                    string secondName = result.Tables[i].Rows[row][col + 2].ToString();
                    string lastName = result.Tables[i].Rows[row][col + 3].ToString();

                    string egn = result.Tables[i].Rows[row][col + 4].ToString();
                    if (egn.Length < 10) egn = (new string('0', 10 - egn.Length)) + egn;


                    string fullName = firstName + " " + secondName + " " + lastName;


                    Student s = new Student()
                    {
                        Name = fullName,
                        Number = Int32.Parse(number),
                        EGN = egn,
                        Class_id = id
                    };

                    students.Add(s);

                    row++;
                }
            }

            StudentsRepository.DeleteAllStudents();
            StudentsRepository.ImportStudents(students);

            excelReader.Close();
        }

        private static int[] ParseClass(string tableName)
        {
            string number = "";
            int letter = 1; ;

            for (int i = 0; i < tableName.Length; i++)
            {
                int n;
                bool isNumber = Int32.TryParse(tableName[i].ToString(), out n);
                if(isNumber) number += n.ToString();
                else
                {
                    if (tableName[i] == 'а') letter = 1;
                    else if (tableName[i] == 'б') letter = 2;
                    else if (tableName[i] == 'в') letter = 3;
                    else if (tableName[i] == 'г') letter = 4;
                    break;
                }
            }

            int[] result = new int[] { Int32.Parse(number), letter };
            return result;
        }
    }
}