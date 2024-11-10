using Microsoft.AspNetCore.Mvc;

namespace OopProject.Controllers
{
    public class User : Controller
    {
        public IActionResult RequestBooks() => View();
    }
}
