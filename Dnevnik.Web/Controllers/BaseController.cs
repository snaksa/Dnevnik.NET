using Dnevnik.Data;
using Dnevnik.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers
{
    //[Logged]
    public class BaseController : Controller
    {
        public Teacher CurrentUser 
        {
            get
            {
                return DB.GetCurrentUser(1);//Int32.Parse(Session["userId"].ToString()));//pass the user id
            }
        }
    }
}