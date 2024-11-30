namespace OopProject.Models
{

    public class Request
    {
        public int Id { get; set; } // Primary Key, auto-incrementing
        public string Status { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; } // Navigation property

        public int BookId { get; set; }
        public virtual Book Book { get; set; } // Navigation property

        // Add the properties to store form data
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    /*    public class Request
        {
            public int Id { get; set; }

            public string Status { get; set; }  // For example: "Pending", "Approved", "Denied"

            // Foreign keys
            public int UserId { get; set; }
            public int BookId { get; set; }

            // Navigation properties
            public User User { get; set; }
            public Book Book { get; set; }
        }*/

}
