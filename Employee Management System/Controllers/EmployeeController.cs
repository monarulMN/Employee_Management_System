using AutoMapper;
using Employee_Management_System.Data;
using Employee_Management_System.Models.Entities;
using Employee_Management_System.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Employee_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        
        public EmployeeController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            const int pageSize = 5;
            int pageNumber = page ?? 1;


            var query = _dbContext.Employees.Include(e => e.Department).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(e => e.FullName.Contains(searchString) || e.Email.Contains(searchString));
            }
            
            var employees = await query.ToListAsync();
            var employeeVMs = _mapper.Map<List<EmployeeViewModel>>(employees);

            
            
            return View(employeeVMs.ToPagedList(pageNumber, pageSize));
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

            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if(!ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);

                _dbContext.Add(employee);
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            var model = _mapper.Map<EmployeeViewModel>(employee);

            model.Departments = _dbContext.Departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var employee = await _dbContext.Employees.FindAsync(model.Id);
                if (employee == null) return NotFound();

                _mapper.Map(model, employee);

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

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _dbContext.Employees.Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound();

            var employeeVM = _mapper.Map<EmployeeViewModel>(employee);
            return View(employeeVM);
        }
    }
}
