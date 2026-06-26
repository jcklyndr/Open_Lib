using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using OopProject.Controllers;
using OopProject.Models;
using OopProject.Services;
using System.Threading.Tasks;
using System.Net;


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
            // for debug lang, para makita sa console
            Console.WriteLine("Books fetched from repository are null.");
            books = new List<Book>(); // Avoid null references by returning an empty list
        }

        return View(books);
    }
    [HttpGet]
    public async Task<IActionResult> AddBooks()
    {
        ViewBag.Categories = await _categoryRepository.GetAllAsync(); // Populate categories
        return View(new Book());
    }
    [HttpPost]
    public async Task<IActionResult> AddBooks(Book model, IFormFile image, List<int> selectedCategoryIds)
    {
        if (!ModelState.IsValid)
        {
            // Return to form if there are errors,di valid model
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View(model);
        }

        // Validate that at least one category is selected
        if (selectedCategoryIds == null || selectedCategoryIds.Count == 0)
        {
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

        // Save
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
        var book = await _bookRepository.GetByIdWithCategoriesAsync(id); // Fetch the book with categories

        if (book == null)
        {
            TempData["ErrorMessage"] = "Book not found.";
            return RedirectToAction("AllBooks");
        }

        // Fetch all
        var categories = await _categoryRepository.GetAllAsync();

        // Get the current category IDs associated with the book
        var currentCategoryIds = (book as Book).BookCategories.Select(bc => bc.CategoryId).ToList();

        // Pass categories and current selected category IDs to the view
        ViewBag.Categories = categories;
        ViewBag.CurrentCategoryIds = currentCategoryIds;
        return View(book);  // Return the book to the view
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
            // Step 1: Update Book Details
            book.BookTitle = updatedBook.BookTitle;
            book.Author = updatedBook.Author;
            book.PublicationYear = updatedBook.PublicationYear;
            book.BookDescription = updatedBook.BookDescription;
            book.AuthorDescription = updatedBook.AuthorDescription;

            // Handle image upload
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

            // Fetch the current categories for the book
            var currentCategories = await _bookCategoryRepository.GetByBookIdAsync(updatedBook.Id);


            // Process categories to remove (unchecked sa UI)
            foreach (var category in currentCategories)
            {
                if (!selectedCategoryIds.Contains(category.CategoryId))
                {
                    // Remove categories
                    await _bookCategoryRepository.DeleteByCompositeKeyAsync(category.BookId, category.CategoryId);
                }
            }

            // Process categories to add (newly checked)
            foreach (var selectedCategoryId in selectedCategoryIds)
            {
                if (!currentCategories.Any(c => c.CategoryId == selectedCategoryId))
                {
                    // Add new categories that are selected but not currently associated
                    var newCategory = new BookCategory
                    {
                        BookId = updatedBook.Id,
                        CategoryId = selectedCategoryId
                    };
                    await _bookCategoryRepository.AddAsync(newCategory);
                }
            }

            // Save changes
            await _bookRepository.UpdateAsync(book);

            TempData["SuccessMessage"] = "Book updated successfully!";
            return RedirectToAction("AllBooks");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"An error occurred while updating the book: {ex.Message}";
            return RedirectToAction("AllBooks");
        }
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

            // Delete the book record along with its related entities
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



