using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace OopProject.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")] //for authentication para di ma-access ng unauthorized
    public class AdminHeaderController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            Console.WriteLine($"IsAuthenticated: {User.Identity?.IsAuthenticated}, Role: Admin - {User.IsInRole("Admin")}");
            if (User.Identity?.IsAuthenticated == true && User.IsInRole("Admin"))
            {
                ViewData["AdminName"] = User.FindFirstValue(ClaimTypes.Name) ?? "Admin";
            }
            else
            {
                Console.WriteLine("Admin not authenticated or not in role.");
            }
        }
    }

}


