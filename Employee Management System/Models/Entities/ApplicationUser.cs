using Microsoft.AspNetCore.Identity;

namespace Employee_Management_System.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        //Additional properties can be added here
        public string? FullName { get; set; }

        public string Designation { get; set; }
    }
}
