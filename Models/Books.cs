using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OopProject.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Book title is required.")]
        public string BookTitle { get; set; }

        [Required(ErrorMessage = "Publication year is required.")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Book description is required.")]
        public string BookDescription { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        public string Author { get; set; }

        public string AuthorDescription { get; set; }

        // Remove CategoryId and Category from here
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    }



}


