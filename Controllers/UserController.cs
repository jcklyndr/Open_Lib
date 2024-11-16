using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;
using Microsoft.AspNetCore.Authorization;

namespace OopProject.Controllers
{
    [Authorize] // Ensure that only authenticated users can access these actions
    public class UserController : UserHeaderController
    {
        private readonly IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult RequestBooks()
        {
            return View();
        }



        // Other user-specific actions can be added here
    }
}
