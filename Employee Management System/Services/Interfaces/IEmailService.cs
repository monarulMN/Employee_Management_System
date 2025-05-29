namespace Employee_Management_System.Services.Interfaces
{
    public interface IEmailService
    {
        Task SenderEmailAsync(string toEmail, string subject, string body);
    }
}
