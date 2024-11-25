using Microsoft.AspNetCore.Mvc;

namespace OopProject.Controllers
{
    public class CategoryBooks : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
