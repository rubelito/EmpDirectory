using System;
using System.Web.Mvc;
using System.Web.Helpers;

namespace BCS.HtmlHelpers
{
    public class ValidateAntiForgeryHeader : FilterAttribute, IAuthorizationFilter
    {
        const string KEY_NAME = "__RequestVerificationToken";
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string clientToken = filterContext.RequestContext.HttpContext.Request.Headers.Get(KEY_NAME);
            if (clientToken == null) throw new HttpAntiForgeryException(String.Format("Header does not contain {0}", KEY_NAME));

            string serverToken = filterContext.HttpContext.Request.Cookies.Get(KEY_NAME).Value;
            if (serverToken == null) throw new HttpAntiForgeryException(String.Format("Cookies does not contain {0}", KEY_NAME));

            AntiForgery.Validate(serverToken, clientToken);
        }
    }
}