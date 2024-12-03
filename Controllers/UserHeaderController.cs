using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace OopProject.Controllers
{
    public class UserHeaderController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            // If the user is authenticated, set the username in ViewData or sa header for Users
            if (User.Identity?.IsAuthenticated == true)
            {
                ViewData["Username"] = User.FindFirstValue(ClaimTypes.Name);
            }
        }
    }
}
