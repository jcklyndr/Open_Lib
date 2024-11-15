namespace OopProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        // Navigation property for books in this category
        public ICollection<Book> Books { get; set; }
    }

}
