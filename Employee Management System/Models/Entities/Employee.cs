using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string   FullName { get; set; }
        public string? Designation { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string   Email { get; set; }
        public string? PhoneNumber { get; set; }

        //[Display(Name = "Photo")]
        //public string? PhotoPath { get; set; }

        //Foreign Key
        public int?  DepartmentId { get; set; }

        //Navigation Property
        public Department? Department { get; set; }

    }
}
