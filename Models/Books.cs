namespace OopProject.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int? PublicationYear { get; set; }
        public string? BookDescription { get; set; }
        public string? BookImage { get; set; }
        public string Author { get; set; }
        public string? AuthorDescription { get; set; }

/*        public Book(int bookid, string title, int categoryid, int? publicationyear, string? bookdescription, string? bookimage, string author, string? authordescription)
        {
            BookId = bookid;
            Title = title;
            CategoryId = categoryid;
            PublicationYear = publicationyear;
            BookDescription = bookdescription;
            BookImage = bookimage;
            Author = author;
            AuthorDescription = authordescription;
        }*/
    }
}
