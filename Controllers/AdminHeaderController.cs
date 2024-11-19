using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace OopProject.Controllers
{
    public class AdminHeaderController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.Identity?.IsAuthenticated == true)
            {
                // Check if the authenticated user has an "Admin" role
                if (User.IsInRole("Admin"))
                {
                    // Extract the admin name from claims and set it in ViewData
                    ViewData["AdminName"] = User.FindFirstValue(ClaimTypes.Name) ?? "Admin";
                }
                else
                {
                    // Redirect non-admin users to an unauthorized page
                    context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
                }
            }
            else
            {
                // Redirect unauthenticated users to the AdminLogin page
                context.Result = new RedirectToActionResult("UserLogin", "Auth", null);
            }
        }
    }
}
