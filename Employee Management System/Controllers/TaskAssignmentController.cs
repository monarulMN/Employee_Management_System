using AutoMapper;
using ClosedXML.Excel;
using Employee_Management_System.Data;
using Employee_Management_System.Models.Entities;
using Employee_Management_System.Models.ViewModels;
using Employee_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Employee_Management_System.Controllers
{
    public class TaskAssignmentController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public TaskAssignmentController(ApplicationDbContext dbContext, 
            UserManager<ApplicationUser> userManager, 
            IMapper mapper,
            IEmailService emailService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            int pageSize = 5;
            var user = await _userManager.GetUserAsync(User);
            var isEmployee = User.IsInRole("Employee");


            var query =  _dbContext.TaskAssignments.Include(t => t.Employee).AsQueryable();

            if (isEmployee && user != null)
            {
                query = query.Where(t => t.EmployeeId == user.Id);
            }

            if(!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t => t.Title.Contains(searchTerm));
            }

            var totalItems = await query.CountAsync();
            var tasks = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.SearchTerm = searchTerm;

            var taskViewModels = _mapper.Map<List<TaskAssignmentViewModel>>(tasks);
            return View(taskViewModels);
        }

        public IActionResult Create()
        {
            ViewBag.Employees = new SelectList(_dbContext.Users.ToList(), "Id", "UserName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskAssignmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employees = new SelectList(_dbContext.Users.ToList(), "Id", "UserName");
                return View(viewModel);
            }

            var entity = _mapper.Map<TaskAssignment>(viewModel);
            _dbContext.TaskAssignments.Add(entity);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _dbContext.TaskAssignments.FindAsync(id);
            if (task == null) return NotFound();

            var viewModel = _mapper.Map<TaskAssignmentViewModel>(task);
            ViewBag.Employees = new SelectList(_dbContext.Users.ToList(), "Id", "UserName");
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskAssignmentViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Employees = new SelectList(_dbContext.Users.ToList(), "Id", "UserName");
                return View(viewModel);
            }

            var existingTask = await _dbContext.TaskAssignments.FindAsync(viewModel.Id);
            if (existingTask == null) return NotFound();


            _mapper.Map(viewModel, existingTask);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _dbContext.TaskAssignments.FindAsync(id);
            if (task == null) return NotFound();

            _dbContext.TaskAssignments.Remove(task);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _dbContext.TaskAssignments.Include(t => t.Employee).FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound();


            var viewModel = _mapper.Map<TaskAssignmentViewModel>(task);
            return View(viewModel);
        }

        public IActionResult ExportToExcel()
        {
            using (var workbook = new XLWorkbook ())
            {
                var worksheet = workbook.Worksheets.Add("Tasks");
                worksheet.Cell(1, 1).Value = "Title";
                worksheet.Cell(1, 2).Value = "Description";
                worksheet.Cell(1, 3).Value = "Employee";
                worksheet.Cell(1, 4).Value = "Due Date";

                var tasks = _dbContext.TaskAssignments.Include(t => t.Employee).ToList();

                for(int i = 0; i < tasks.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = tasks[i].Title;
                    worksheet.Cell(i + 2, 2).Value = tasks[i].Description;
                    worksheet.Cell(i + 2, 3).Value = tasks[i].Employee?.UserName;
                    worksheet.Cell(i + 2, 4).Value = tasks[i].DueDate.ToShortDateString();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tasks.xlsx");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Assign(TaskAssignmentViewModel model)
        {
            var task = new TaskAssignment
            {
                TaskName = model.TaskName,
                Description = model.Description,
                EmployeeId = model.EmployeeId,
                AssignedDate = DateTime.Now
            };

            _dbContext.TaskAssignments.Add(task);
            await _dbContext.SaveChangesAsync();

            var employee = await _dbContext.Users.FindAsync(model.EmployeeId);
            if(employee != null)
            {
                await _emailService.SenderEmailAsync(
                    employee.Email,
                    "New Task Assigned",
                    $"Dear {employee.FullName}, <br/><br/> You have been assigned a new task: " +
                        "<strong>{task.TaskName}</strong>.<br/>Please check your dashboard.<br/>" +
                        "<br/>Best regards,<br/>EMS System "
                );
            }

            return RedirectToAction("Index");
        }
    }
}
