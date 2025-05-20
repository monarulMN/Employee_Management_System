using Employee_Management_System.Models.Entities;
using Employee_Management_System.Repositories.Interfaces;
using Employee_Management_System.Services.Interfaces;

namespace Employee_Management_System.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;        
        }

        public async Task<List<Employee>> GetAllEmployeesAsync() => await _employeeRepository.GetAllAsync();
    }
}
