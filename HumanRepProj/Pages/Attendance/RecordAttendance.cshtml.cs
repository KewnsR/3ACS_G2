using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HumanRepProj.Data;
using HumanRepProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HumanRepProj.Pages.Attendance
{
    public class RecordAttendanceModel : PageModel
    {
        private readonly ILogger<RecordAttendanceModel> _logger;
        private readonly ApplicationDbContext _context;

        public RecordAttendanceModel(
            ILogger<RecordAttendanceModel> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public List<AttendanceRecord> AttendanceRecords { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(DateTime? date)
        {
            var userEmail = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogWarning("Session expired or user not logged in.");
                return RedirectToPage("/Login");
            }

            _logger.LogInformation($"User {userEmail} accessed the Record Attendance page.");

            SelectedDate = date ?? DateTime.Today;

            // Load attendance records for selected date
            AttendanceRecords = await _context.AttendanceRecords
                .Include(a => a.Employee)
                .Where(a => a.AttendanceDate.Date == SelectedDate.Date)
                .ToListAsync();

            // Load all employees for dropdown
            Employees = await _context.Employees.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int employeeId, string status)
        {
            var userEmail = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogWarning("Session expired or user not logged in.");
                return RedirectToPage("/Login");
            }

            if (employeeId <= 0 || string.IsNullOrEmpty(status))
            {
                ModelState.AddModelError("", "Please select a valid employee and status.");
                return Page();
            }

            var today = DateTime.Today;

            // Check if record already exists
            var existingRecord = await _context.AttendanceRecords
                .FirstOrDefaultAsync(r => r.EmployeeID == employeeId && r.AttendanceDate.Date == today.Date);

            if (existingRecord != null)
            {
                // Update existing record
                existingRecord.Status = status;
                existingRecord.AttendanceDate = today;
                _context.AttendanceRecords.Update(existingRecord);
                _logger.LogInformation($"Attendance record updated for Employee ID {employeeId}");
            }
            else
            {
                // Create new record
                var newRecord = new AttendanceRecord
                {
                    EmployeeID = employeeId,
                    Status = status,
                    AttendanceDate = today
                };
                await _context.AttendanceRecords.AddAsync(newRecord);
                _logger.LogInformation($"New attendance record added for Employee ID {employeeId}");
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}