using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
/*using Microsoft.AspNetCore.Authentication.Cookies;*/

namespace OopProject.Controllers
{
    public class AuthController : UserHeaderController
    {
        private readonly IRepository<User> _userRepository;
        public AuthController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // SignUp View for Users
        public IActionResult UserSignup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserSignup(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetAllAsync();
                if (existingUser.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(user);
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); //hashing
                await _userRepository.AddAsync(user);

                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("UserLogin");
            }

            return View(user);
        }
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(string Username, string Password)
        {
            try
            {
                var user = (await _userRepository.GetAllAsync()).FirstOrDefault(u => u.Username == Username);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid username.");
                    return View();
                }

                if (!BCrypt.Net.BCrypt.Verify(Password, user.Password))
                {
                    ModelState.AddModelError(string.Empty, "Invalid password.");
                    return View();
                }

                // Successful login
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "User")  // Added the User role claim
                };

                var claimsIdentity = new ClaimsIdentity(claims, "UserScheme");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync("UserScheme", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                //debugging
                Console.WriteLine($"Error during login: {ex.Message}");
                return View();
            }
        }
        public async Task<IActionResult> Logout()
        {
            // Retrieve role from claims
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == "User")
            {
                await HttpContext.SignOutAsync("UserScheme");  // Use the correct scheme for User
                TempData["SuccessMessage"] = "User logged out successfully.";
                return RedirectToAction("UserLogin", "Auth");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid role specified.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
