using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;

namespace OopProject.Controllers
{
    [Authorize(AuthenticationSchemes = "UserScheme")]
    public class BooksController : UserHeaderController
    {
        private readonly IRepository<Book> _bookRepository;

        public BooksController(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> PerCategory(int categoryId)
        {
            var books = await _bookRepository.GetAllWithCategoriesAsync();
            var booksInCategory = books
                .OfType<Book>() // Ensures casting from generic repository
                .Where(b => b.BookCategories.Any(bc => bc.CategoryId == categoryId))
                .ToList();

            ViewBag.CategoryId = categoryId; // Pass the category ID for dynamic display
            return View(booksInCategory);
        }

        public async Task<IActionResult> BookDetails(int id)
        {
            // Fetch the book details based on the ID
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found!";
                return RedirectToAction("PerCategory", new { categoryId = ViewBag.CategoryId }); // Redirect back to the category view
            }

            // Pass the book to the view
            return View(book);
        }


        [HttpPost]
        public IActionResult RequestBooks(string Name, string Email, string PhoneNum)
        {
            TempData["SuccessMessage"] = "Your request has been submitted successfully!";
            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
