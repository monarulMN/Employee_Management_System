using Employee_Management_System.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Full name is required")]
        [StringLength(150)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Display(Name = "Designation")]
        [StringLength(100)]
        public string? Designation { get; set; }


        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public DateTime? JoiningDate { get; set; }


        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Email { get; set; }


        [Display(Name = "Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }


        //[Display(Name = "Photo")]
        //public string? PhotoPath { get; set; }


        //Foreign Key
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        //Navigation Property
        public List<SelectListItem> Departments { get; set; }

        public string? DepartmentName { get; set; }
    }
}
