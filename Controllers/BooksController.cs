using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using RequestModel = OopProject.Models.Request;

namespace OopProject.Controllers
{
    [Authorize(AuthenticationSchemes = "UserScheme")]
    public class BooksController : UserHeaderController
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Request> _requestRepository; // Inject the repository for Request
        private readonly IRepository<User> _userRepository;

        public BooksController(IRepository<Book> bookRepository, IRepository<Request> requestRepository, IRepository<User> userRepository)
        {
            _bookRepository = bookRepository;
            _requestRepository = requestRepository; // Initialize the repository for Request
            _userRepository = userRepository;
        }

        public async Task<IActionResult> PerCategory(int categoryId)
        {
            var books = await _bookRepository.GetAllWithCategoriesAsync();
            var booksInCategory = books
                .OfType<Book>() // casting from generic repository
                .Where(b => b.BookCategories.Any(bc => bc.CategoryId == categoryId))
                .ToList();

            ViewBag.CategoryId = categoryId; // Pass the category ID for dynamic display
            return View(booksInCategory);
        }

        public async Task<IActionResult> BookDetails(int id)
        {
            // Fetch the book details
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found!";
                return RedirectToAction("PerCategory", new { categoryId = ViewBag.CategoryId }); // Redirect back to the category view
            }

            // Pass sa view
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> RequestBooks(int bookId, string name, string email, string phone_number)
        {
            // Make sure that phone_number is not null or empty before creating the request
            if (string.IsNullOrEmpty(phone_number))
            {
                TempData["ErrorMessage"] = "Phone number is required.";
                return RedirectToAction("BookDetails", new { id = bookId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to request a book.";
                return RedirectToAction("BookDetails", new { id = bookId });
            }

            var book = await _bookRepository.GetByIdAsync(bookId);
            var user = await _userRepository.GetByIdAsync(int.Parse(userId));

            if (book == null || user == null)
            {
                TempData["ErrorMessage"] = "Invalid book or user.";
                return RedirectToAction("BookDetails", new { id = bookId });
            }

            // Create the request object
            var bookRequest = new Request
            {
                BookId = book.Id,
                UserId = user.Id,
                Status = RequestModel.RequestStatus.Pending,
                Book = book,
                User = user,
                Name = name,
                Email = email,
                PhoneNumber = phone_number
            };


            // Save the request using the repository
            await _requestRepository.AddAsync(bookRequest);

            TempData["SuccessMessage"] = "Your request has been submitted successfully!";
            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
