using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using OopProject.Controllers;
using OopProject.Models;
using OopProject.Services;
using System.Threading.Tasks;



// Action to display the add book form
public class BooksAdminController : AdminHeaderController
{
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<BookCategory> _bookCategoryRepository;

    public BooksAdminController(IRepository<Category> categoryRepository,
                                 IRepository<Book> bookRepository,
                                 IRepository<BookCategory> bookCategoryRepository)
    {
        _categoryRepository = categoryRepository;
        _bookRepository = bookRepository;
        _bookCategoryRepository = bookCategoryRepository;
    }

    public async Task<IActionResult> AllBooks()
    {
        // Fetch all books with categories
        var books = await _bookRepository.GetAllWithCategoriesAsync();

        // Check for null and log/debug if necessary
        if (books == null)
        {
            // Optional: Log the issue for further inspection
            Console.WriteLine("Books fetched from repository are null.");
            books = new List<Book>(); // Avoid null references by returning an empty list
        }

        return View(books);
    }
    public async Task<IActionResult> AddBooks(Book model, IFormFile image, List<int> selectedCategoryIds)
    {
        if (!ModelState.IsValid)
        {
            // Return to the form if there are validation errors
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View(model);
        }

        // Handle the image upload if an image is provided
        if (image != null)
        {
            var imagePath = Path.Combine("wwwroot", "images", "books", image.FileName);
            if (!Directory.Exists(Path.Combine("wwwroot", "images", "books")))
            {
                Directory.CreateDirectory(Path.Combine("wwwroot", "images", "books"));
            }

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            model.Image = "/images/books/" + image.FileName; // Store the relative image path
        }

        // Save the book to the database
        await _bookRepository.AddAsync(model);

        // Ensure the book has an Id (it gets assigned after saving the book)
        var bookId = model.Id;

        // Add entries to the BookCategory table for each selected category
        if (selectedCategoryIds != null && selectedCategoryIds.Count > 0)
        {
            foreach (var categoryId in selectedCategoryIds)
            {
                var bookCategory = new BookCategory
                {
                    BookId = bookId,   // BookId from the model
                    CategoryId = categoryId
                };

                await _bookCategoryRepository.AddAsync(bookCategory);
            }
        }

        TempData["SuccessMessage"] = "Book added successfully!";
        return RedirectToAction("AllBooks");
    }



    [HttpGet]
    public async Task<IActionResult> UpdateBooks(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            TempData["ErrorMessage"] = "Book not found.";
            return RedirectToAction("AllBooks");
        }

        // Get the categories associated with the book
        var selectedCategoryIds = book.BookCategories.Select(bc => bc.CategoryId).ToList();

        // Get all categories for the dropdown
        var allCategories = await _categoryRepository.GetAllAsync();
        ViewBag.Categories = allCategories;

        // Store selected categories for view binding
        ViewData["SelectedCategoryIds"] = selectedCategoryIds;

        return View(book);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateBooks(Book updatedBook, IFormFile? image, List<int> selectedCategoryIds)
    {
        if (!ModelState.IsValid)
        {
            return View(updatedBook);
        }

        var book = await _bookRepository.GetByIdAsync(updatedBook.Id);
        if (book == null)
        {
            TempData["ErrorMessage"] = "Book not found.";
            return RedirectToAction("AllBooks");
        }

        try
        {
            // Update book properties
            book.BookTitle = updatedBook.BookTitle;
            book.Author = updatedBook.Author;
            book.PublicationYear = updatedBook.PublicationYear;
            book.BookDescription = updatedBook.BookDescription;
            book.AuthorDescription = updatedBook.AuthorDescription;

            // Handle image update (if applicable)
            if (image != null)
            {
                if (!string.IsNullOrEmpty(book.Image))
                {
                    var oldImagePath = Path.Combine("wwwroot", book.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var newImagePath = Path.Combine("wwwroot", "images", "books", image.FileName);
                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                book.Image = "/images/books/" + image.FileName;
            }

            var existingCategories = book.BookCategories.ToList();
            foreach (var category in existingCategories)
            {
                await _bookCategoryRepository.DeleteByCompositeKeyAsync(category.BookId, category.CategoryId);
            }
            // Debugging: Check if categories are removed
            var categoriesAfterDelete = book.BookCategories.ToList();
            Console.WriteLine($"Categories remaining: {categoriesAfterDelete.Count}");



            if (selectedCategoryIds != null && selectedCategoryIds.Any())
            {
                foreach (var categoryId in selectedCategoryIds)
                {
                    var bookCategory = new BookCategory
                    {
                        BookId = book.Id,
                        CategoryId = categoryId
                    };
                    await _bookCategoryRepository.AddAsync(bookCategory);
                }
            }


            // Save updated book
            await _bookRepository.UpdateAsync(book);

            TempData["SuccessMessage"] = "Book updated successfully!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error updating book: {ex.Message}";
        }

        return RedirectToAction("AllBooks");
    }


    [HttpPost]
    public async Task<IActionResult> DeleteBookConfirmed(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
        {
            TempData["ErrorMessage"] = "Book not found.";
            return RedirectToAction("AllBooks");
        }

        try
        {
            // Delete the associated image if it exists
            if (!string.IsNullOrEmpty(book.Image))
            {
                var imagePath = Path.Combine("wwwroot", book.Image.TrimStart('/')); // Convert relative path to physical path
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Delete the book record
            await _bookRepository.DeleteAsync(id);

            TempData["SuccessMessage"] = "Book and its image deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting book: {ex.Message}";
        }

        return RedirectToAction("AllBooks");
    }

}



