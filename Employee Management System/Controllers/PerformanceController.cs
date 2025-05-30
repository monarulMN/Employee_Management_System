 using AutoMapper;
using Employee_Management_System.Data;
using Employee_Management_System.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Controllers
{
    public class PerformanceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public PerformanceController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Report()
        {
            var model = new PerformanceReportViewModel
            {
                Employees = _dbContext.Users
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id,
                        Text = e.FullName
                    }).ToList(),
                FilteredTasks = new List<TaskAssignmentViewModel>()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Report(PerformanceReportViewModel model)
        {
            var query = _dbContext.TaskAssignments
                .Include(t => t.Employee)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.EmployeeId))
                query = query.Where(t => t.EmployeeId == model.EmployeeId);

            if (!string.IsNullOrEmpty(model.Designation))
                query = query.Where(t => t.Employee.Designation == model.Designation);

            if (model.FromDate.HasValue)
                query = query.Where(t => t.AssignedDate >= model.FromDate);

            if (model.ToDate.HasValue)
                query = query.Where(t => t.AssignedDate <= model.ToDate);

            var tasks = await query
                .Select(t => new TaskAssignmentViewModel
                {
                    Id = t.Id,
                    TaskName = t.TaskName,
                    Description = t.Description,
                    AssignedTo = t.Employee.FullName,
                    Designation = t.Employee.Designation,
                    AssignedDate = t.AssignedDate,
                    IsCompleted = t.Description == "Completed"
                })
                .ToListAsync();

            model.Employees = _dbContext.Users
                .Select(e => new SelectListItem
                {
                    Value = e.Id,
                    Text = e.FullName
                }).ToList();

            model.FilteredTasks = tasks;

            return View(model);
        }
    }
}
