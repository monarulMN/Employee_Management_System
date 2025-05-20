using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee_Management_System.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public List<SelectListItem> DepartmentList { get; set; }
    }
}
