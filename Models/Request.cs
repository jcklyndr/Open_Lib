namespace OopProject.Models
{
    public class Request
    {
        public int Id { get; set; }

        public string Status { get; set; }  // For example: "Pending", "Approved", "Denied"

        // Foreign keys
        public int UserId { get; set; }
        public int BookId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Book Book { get; set; }
    }

}
