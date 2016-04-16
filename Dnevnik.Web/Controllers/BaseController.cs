using Dnevnik.Data;
using Dnevnik.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers
{
    [Logged]
    public class BaseController : Controller
    {
        public Teacher CurrentUser 
        {
            get
            {
                return DB.GetCurrentUser(Int32.Parse(Request.Cookies["userId"].Value));
            }
        }
    }
}