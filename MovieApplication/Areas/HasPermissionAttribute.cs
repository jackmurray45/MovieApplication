using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using MovieApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using MovieApplication.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MovieApplication.Areas
{
    public class HasPermissionAttribute : ActionFilterAttribute
    {
        private string _permission;

        public HasPermissionAttribute(string permission)
        {
            this._permission = permission;

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CheckForPermissionAsync(filterContext).GetAwaiter().GetResult())
            {
                // If this user does not have the required permission then redirect to login page
                var url = new UrlHelper(filterContext);
                var loginUrl = url.Content(filterContext.HttpContext.User == null ? "/Identity/Account/Login" : "/");
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }

        private async Task<bool> CheckForPermissionAsync(ActionExecutingContext filterContext)
        {

            ApplicationDbContext context = filterContext.HttpContext.RequestServices.GetService<ApplicationDbContext>();
            UserManager<ApplicationUser> userManager = context.GetService<UserManager<ApplicationUser>>();
            ApplicationUser user = await userManager.GetUserAsync(filterContext.HttpContext.User);
            return await userManager.IsInRoleAsync(user, "Admin");

        }


    }
}
