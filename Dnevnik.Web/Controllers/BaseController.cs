using Dnevnik.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dnevnik.Web.Controllers
{
    public class BaseController : Controller
    {
        public Teacher CurrentUser 
        {
            get
            {
                return DB.GetCurrentUser(1);//pass the user id
            }
        }
    }
}