namespace OopProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? PhoneNum { get; set; }
        public string Password { get; set; }

        // Navigation property
        public ICollection<Request>? Requests { get; set; }
    }

}
