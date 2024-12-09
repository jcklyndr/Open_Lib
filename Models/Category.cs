namespace OopProject.Models;

public class Category
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public string? Image { get; set; }
    public string Description { get; set; }

    // This is where you access the many-to-many relationship
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}

