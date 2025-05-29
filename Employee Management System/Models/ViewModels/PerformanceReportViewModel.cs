using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models.ViewModels
{
    public class PerformanceReportViewModel
    {
        public string? EmployeeId { get; set; }
        public string Designation { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        //[Required]
        //public int EmployeeId { get; set; }
        //public string? EmployeeName { get; set; }

        //public List<SelectListItem> Employees { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Employees { get; set; }
        public List<TaskAssignmentViewModel> FilteredTasks { get; set; }
    }
}
