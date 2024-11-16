using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace OopProject.Controllers
{
    public class AuthController : UserHeaderController
    {
        private readonly IRepository<User> _userRepository;

        public AuthController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // SignUp View for  redirect into UserSignup razor
        public IActionResult UserSignup()
        {
            return View();
        }

        // POST: /Auth/UserSignup
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

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _userRepository.AddAsync(user);

                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("UserLogin");
            }

            return View(user);
        }

        // Login View for redirect to UserLogin View
        public IActionResult UserLogin()
        {
            return View();
        }

        // POST: /Auth/UserLogin
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
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log error for debugging
                Console.WriteLine($"Error during login: {ex.Message}");
                return View();
            }
        }


        // Logout Action
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }
    }
}
