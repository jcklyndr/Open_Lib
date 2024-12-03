using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;

namespace OopProject.Controllers
{
    public class HomeController : UserHeaderController  //extend all to Header as it holds/set the ViewData of the username that will be used in the
                                                      // partial view header which is used in every razor below. So pag authenticate user                                                   // madidisplay name sa header and to those page na may partial view header 
    {

        private readonly IRepository<Category> _categoryRepository;
        public HomeController( IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            // Fetch the categories
            var categories = await _categoryRepository.GetAllAsync(); 

            // Pass the categories sa view
            ViewBag.Categories = categories;

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
