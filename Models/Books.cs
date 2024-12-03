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

        [Required(ErrorMessage = "Image is required.")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Description for author is required.")]
        public string AuthorDescription { get; set; }

        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    }



}


