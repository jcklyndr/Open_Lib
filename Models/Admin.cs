namespace OopProject.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }

        public Admin(int adminid, string adminname, string adminemail, string adminpassword)
        {
            AdminId = adminid;
            AdminName = adminname;
            AdminEmail = adminemail;
            AdminPassword = adminpassword;
        } 
    }
}
