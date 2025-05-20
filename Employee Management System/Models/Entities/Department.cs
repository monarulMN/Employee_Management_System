using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models.Entities
{
    public class Department
    {
        public int Id { get; set; }


        [Required(ErrorMessage ="Department name is required")]
        [StringLength(100)]
        [Display(Name ="Department Name")]
        public string Name { get; set; }


        // Navigation Property: One Department → Many Employees
        public ICollection<Employee> Employees { get; set; } 
    }
}
