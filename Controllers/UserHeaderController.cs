using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;


namespace OopProject.Controllers
{
    public class UserHeaderController : Controller
    {
        // This method is executed before any action method in the controller
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // Extract the username from the user's claims (to display the name of the User)
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                // Set the username in ViewData to be accessed in the views
                ViewData["Username"] = username;
            }
            else
            {
                // If the user is not authenticated, ViewData["Username"] is not set, and it defaults to null.
                ViewData["Username"] = null;
            }
        }
    }
}
