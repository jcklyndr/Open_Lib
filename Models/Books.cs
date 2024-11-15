namespace OopProject.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public int PublicationYear { get; set; }
        public string BookDescription { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
        public string AuthorDescription { get; set; }

        // Foreign key to Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }  // Navigation property
    }

}
