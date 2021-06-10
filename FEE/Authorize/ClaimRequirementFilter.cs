using FEE.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEE.Authorize
{
    public class ClaimRequirementFilter : AuthorizeAttribute
    {
        public string Function { get; set; }
        public string Command { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (UserSession)HttpContext.Current.Session["USER"];
            if (session == null)
            {
                return false;
            }

            List<string> permissions = this.GetCredentialByLoggedInUser(session.Id);

            if (permissions.Contains(Function + "_" + Command))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/_Layouts.cshtml"
            };
        }
        private List<string> GetCredentialByLoggedInUser(int userId)
        {
            var credentials = (List<string>)HttpContext.Current.Session["PERMISSION"];
            return credentials;
        }

    }
}