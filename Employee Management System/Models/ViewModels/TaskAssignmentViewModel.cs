using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models.ViewModels
{
    public class TaskAssignmentViewModel
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public List<SelectListItem> Employees { get; set; } = new List<SelectListItem>();
    }
}
