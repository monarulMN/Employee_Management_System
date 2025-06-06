﻿using Employee_Management_System.Data;
using Employee_Management_System.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Employee_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public DashboardController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;    
        }
        public async Task<IActionResult> Index()
        {
            var totalEmployees = await _dbContext.Users.CountAsync();
            var totalTasks = await _dbContext.TaskAssignments.CountAsync();
            var completedTasks = await _dbContext.TaskAssignments.CountAsync(t => t.Description == "Completed");
            var totalDepartments = await _dbContext.Departments.CountAsync();

            var chartData = await _dbContext.TaskAssignments
                .GroupBy(t => t.Description)
                .Select(g => new TaskChartData { Description = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.TotalEmployees = totalEmployees;
            ViewBag.TotalTasks = totalTasks;
            ViewBag.CompletedTasks = completedTasks;
            ViewBag.TotalDepartments = totalDepartments;

            ViewBag.ChartData = chartData;

            return View();
        }
    }
}
