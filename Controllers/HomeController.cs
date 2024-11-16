using Microsoft.AspNetCore.Mvc;

namespace OopProject.Controllers
{
    public class HomeController : UserHeaderController  //extend all to Header as it holds/set the ViewData of the username that will be used in the
                                                      // partial view header which is used in every razor below. So pag authenticate user
                                                      // madidisplay name sa header and to those page na may partial view header 
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }


}
