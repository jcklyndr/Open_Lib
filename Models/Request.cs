namespace OopProject.Models
{
    public class Request
    {
        public int RequestId { get; set; }
        public int UserId {  get; set; }
        public int BookId { get; set; }
        public string Status {  get; set; }

        public Request (int requestId, int userId, int bookId, string status)
        {
            RequestId = requestId;
            UserId = userId;
            BookId = bookId;
            Status = status;
        }
    }
}
