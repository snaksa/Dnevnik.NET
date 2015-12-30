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
    }
}