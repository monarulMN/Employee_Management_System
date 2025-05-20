using Employee_Management_System.Data;
using Employee_Management_System.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        
        public EmployeeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var employees = await _dbContext.Employees.Include(e => e.Department).ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = _dbContext.Departments.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Add(employee);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = _dbContext.Departments.ToList();
            return View(employee);
        }
    }
}
