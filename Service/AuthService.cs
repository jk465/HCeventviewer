using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace HCeventviewer.Service
{
    public class AuthService : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var FullUserName = actionContext.RequestContext.Principal.Identity.Name;

            var UserName = FullUserName.Replace("SYSTEMS\\", "");

            bool IsAuthorizedUser = ConfigService.AuthorizedUsers.Any(user => user.Equals(UserName, StringComparison.OrdinalIgnoreCase));

            if (IsAuthorizedUser)
                return true;

            return false;
        }
    }
}