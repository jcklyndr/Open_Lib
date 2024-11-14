namespace OopProject.Models
{
    public class User
    {
        public int UserId { get; set; } 
        public string UserName { get; set; }
        public string Email { get; set; } //unique
        public string? PhoneNum { get; set; } // nullable, kaya may ?
        public string Password { get; set; }
    }
}
