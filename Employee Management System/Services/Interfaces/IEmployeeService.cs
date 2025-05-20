using Employee_Management_System.Models.Entities;

namespace Employee_Management_System.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
    }
}
