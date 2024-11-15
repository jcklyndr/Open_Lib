namespace OopProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? PhoneNum { get; set; } // ? for optional or in database it is Null
        public string Password { get; set; }

        // Navigation property for requests made by the user
        public ICollection<Request>? Requests { get; set; }
    }

}
