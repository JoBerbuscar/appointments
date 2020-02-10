﻿using System.Web;
using System.Web.Mvc;

namespace GAP.Appointments.Web.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return ((CurrentUser != null && !CurrentUser.IsInRole(Roles)) || CurrentUser == null) ? false : true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;

            if (CurrentUser == null)
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
                        controller = "Account",
                        action = "Login",
                    }
                    ));
            }
            else
            {
                if (AuthorizeCore(new HttpContextWrapper(HttpContext.Current)))
                {
                    routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                     (new
                     {
                         controller = "ErroLogin",
                         action = "AccessDenied"
                     }
                     ));
                }
            }

            filterContext.Result = routeData;
        }

    }
}