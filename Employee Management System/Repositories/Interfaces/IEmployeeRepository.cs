using Employee_Management_System.Models.Entities;

namespace Employee_Management_System.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        //Task<Employee?> GetByIdAsync(int id);
        //Task AddAsync(Employee employee);
        //Task UpdateAsync(Employee employee);
        //Task DeleteAsync(int id);
    }
}
