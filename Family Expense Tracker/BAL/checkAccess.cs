using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Family_Expense_Tracker.BAL
{
    public class CheckAccess : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            // Check if the 'isLogin' cookie exists and is set to "true"
            var isLoginCookie = filterContext.HttpContext.Request.Cookies["isLogin"];
            if (string.IsNullOrEmpty(isLoginCookie) || isLoginCookie != "true")
            {
                // If not logged in, return Unauthorized status
               // filterContext.Result = new UnauthorizedResult();
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Expires"] = "-1";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            base.OnResultExecuting(context);
        }
    }
}
