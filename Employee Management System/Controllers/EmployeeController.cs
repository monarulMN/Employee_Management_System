using Employee_Management_System.Data;
using Employee_Management_System.Models.Entities;
using Employee_Management_System.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var model = new EmployeeViewModel
            {
                Departments = _dbContext.Departments.Select(d => new SelectListItem
                {
                    Value =d.Id.ToString(),
                    Text = d.Name
                }).ToList()
            };

            //ViewBag.Departments = _dbContext.Departments.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if(ModelState.IsValid)
            {
                var employee = new Employee
                {
                    FullName = model.FullName,
                    Designation = model.Designation,
                    JoiningDate = model.JoiningDate,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    DepartmentId = model.DepartmentId
                };

                _dbContext.Add(employee);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.Departments = _dbContext.Departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();
            //ViewBag.Departments = _dbContext.Departments.ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Designation = employee.Designation,
                JoiningDate = employee.JoiningDate,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                Departments = _dbContext.Departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _dbContext.Employees.FindAsync(model.Id);
                if (employee == null) return NotFound();

                employee.FullName = model.FullName;
                employee.Designation = model.Designation;
                employee.JoiningDate = model.JoiningDate;
                employee.PhoneNumber = model.PhoneNumber;
                employee.Email = model.Email;
                employee.DepartmentId = model.DepartmentId;

                _dbContext.Employees.Update(employee);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.Departments = _dbContext.Departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if(employee == null) return NotFound();

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
