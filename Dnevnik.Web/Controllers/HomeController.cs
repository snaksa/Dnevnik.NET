namespace Dnevnik.Web.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using Dnevnik.Data;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Dnevnik.Web.ViewModels;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("This is the index!");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            int user = DB.LoginUser(username, password);
            if (user == -1)
            {
                //redirect back
                return View();
            }
            else
            {
                Session["userId"] = user;
                return RedirectToAction("Show", "Students");
            }
        }
    }
}