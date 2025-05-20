using Employee_Management_System.Models.Entities;

namespace Employee_Management_System.Data
{
    public class SeedData
    {
        public static void Initialize(ApplicationDbContext dbContext)
        {
            if (!dbContext.Departments.Any())
            {
                dbContext.Departments.AddRange
                (
                    new Department { Name ="HR"},
                    new Department { Name ="IT"},
                    new Department { Name ="Accounts"}
                );
                dbContext.SaveChanges();
            }
        }
    }
}
