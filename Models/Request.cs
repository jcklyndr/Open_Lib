namespace OopProject.Models
{

    public class Request
    {
        public int Id { get; set; } // Primary Key, auto-incrementing
        public RequestStatus Status { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; } // Navigation property

        public int BookId { get; set; }
        public virtual Book Book { get; set; } // Navigation property

        // Add the properties to store form data
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public enum RequestStatus
        {
            Pending,
            Processing,
            BookedSuccessfully,
            Failed
        }
    }

}
