namespace OopProject.Models;

public class Category
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public string? Image { get; set; }  // Make Image nullable if not always required
    public string Description { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}