using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dnevnik.Web.Filters
{
    public class LoggedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            filterContext.HttpContext.Session.Timeout = 30;

            var id = filterContext.HttpContext.Session["userId"];
            var admin = filterContext.HttpContext.Session["adminUser"];
            if (id == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                     new RouteValueDictionary {
                     { "Controller", "Home" },
                     { "Action", "Login" } });
            }
            if (admin != null)
            {
                filterContext.Result = new RedirectToRouteResult(
                     new RouteValueDictionary {
                     { "Controller", "Teachers" },
                     { "Action", "Show" } });
            }
        }
    }
}