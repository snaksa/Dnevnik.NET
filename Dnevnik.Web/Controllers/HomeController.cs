namespace Dnevnik.Web.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using Dnevnik.Data;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Dnevnik.Web.ViewModels;
    using System;
    using System.Net.Mail;
    using System.Text;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("This is the index!");
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["userId"] != null)
            {
                if (Session["adminUser"] != null) return RedirectToAction("Show", "Teachers");
                else return RedirectToAction("Show", "Students");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (username == null || password == null)
            {
                TempData["error"] = "1";
                return View();
            }
            Teacher user = DB.LoginUser(username, password);
            if (user == null)
            {
                TempData["error"] = "1";
                return View();
            }
            else
            {
                Session["userId"] = user.Id;
                if (user.IsAdmin == 1) Session["adminUser"] = "1";
                Session["username"] = user.Name;
                return RedirectToAction("Show", "Students");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["userId"] = null;
            Session["adminUser"] = null;
            Session["username"] = null;
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Settings()
        {
            if (Session["userId"] == null) return RedirectToAction("Login");
            if(Session["adminUser"] != null) return RedirectToAction("Login");
            int id = Int32.Parse(Session["userId"].ToString());
            var teacher = DB.GetCurrentUser(id);
            return View("TeacherSettings", teacher);
        }

        [HttpGet]
        public ActionResult AdminSettings()
        {
            if (Session["userId"] == null) return RedirectToAction("Login");
            if (Session["adminUser"] == null) return RedirectToAction("Login");
            int id = Int32.Parse(Session["userId"].ToString());
            var teacher = DB.GetCurrentUser(id);
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSettings(Teacher t)
        {
            try
            {
                DB.UpdateTeacherSettings(t);
                Session["username"] = t.Name;
                TempData["success"] = "1";
            }
            catch (Exception ex)
            {
                TempData["success"] = "0";
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendPassword(string email)
        {
            var user = DB.GetUserByEmail(email);
            if (user != null)
            {
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("sou.kubrat.dnevnik@gmail.com", "Prikazka12");

                string body = user.Name + ", паролата ви за електронния дневник на СОУ 'Христо Ботев' - гр. Кубрат е: " + user.Password;

                MailMessage mm = new MailMessage("sou.kubrat.dnevnik@gmail.com", user.Email, "Парола за електронен дневник", body);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);

                TempData["sent"] = "1";
            }
            else
            {
                TempData["sent"] = "0";
            }


            return RedirectToAction("Login");
        }
    }
}