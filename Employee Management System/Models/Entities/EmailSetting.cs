namespace Employee_Management_System.Models.Entities
{
    public class EmailSetting
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
