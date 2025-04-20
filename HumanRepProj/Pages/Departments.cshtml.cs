using HumanRepProj.Data;
using HumanRepProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace HumanRepProj.Pages
{
    public class DepartmentsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DepartmentsModel> _logger;

        public DepartmentsModel(ApplicationDbContext context, ILogger<DepartmentsModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<DepartmentViewModel> Departments { get; set; } = new List<DepartmentViewModel>();

        [BindProperty]
        public DepartmentInputModel Input { get; set; }

        public class DepartmentViewModel
        {
            public int DepartmentID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int EmployeeCount { get; set; }
            public decimal Performance { get; set; }
            public DateTime DateCreated { get; set; }
            public string FormattedDate => DateCreated.ToString("dd MMM yyyy");
            public decimal Budget { get; set; }
            public string FormattedBudget => Budget.ToString("C");
            public string Status { get; set; }
            public string StatusClass => Status == "Active" ? "bg-success" : "bg-secondary";
        }

        public class DepartmentInputModel
        {
            public int? DepartmentID { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
            public string Name { get; set; }

            [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
            public string Description { get; set; }

            [Range(0, 100, ErrorMessage = "Performance must be between 0 and 100.")]
            public decimal Performance { get; set; }

            [DataType(DataType.Date)]
            public DateTime DateCreated { get; set; } = DateTime.Now;

            [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive value.")]
            public decimal Budget { get; set; }

            [Required]
            public string Status { get; set; } = "Active";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userEmail = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogWarning("Session expired or user not logged in.");
                return RedirectToPage("/Login");
            }

            _logger.LogInformation($"User {userEmail} accessed the Departments page.");
            await LoadDepartments();
            return Page();
        }

        private async Task LoadDepartments()
        {
            Departments = await _context.Departments
                .Select(d => new DepartmentViewModel
                {
                    DepartmentID = d.DepartmentID,
                    Name = d.Name,
                    Description = d.Description ?? string.Empty,
                    EmployeeCount = _context.Employees.Count(e => e.DepartmentID == d.DepartmentID),
                    Performance = d.Performance,
                    DateCreated = d.DateCreated,
                    Budget = d.Budget,
                    Status = d.Status
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartments();
                return Page();
            }

            var department = new Department
            {
                Name = Input.Name,
                Description = Input.Description,
                Performance = Input.Performance,
                DateCreated = Input.DateCreated,
                Budget = Input.Budget,
                Status = Input.Status
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartments();
                return Page();
            }

            if (!Input.DepartmentID.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Department ID is required for editing.");
                await LoadDepartments();
                return Page();
            }

            var department = await _context.Departments.FindAsync(Input.DepartmentID.Value);

            if (department == null)
            {
                ModelState.AddModelError(string.Empty, "Department not found.");
                await LoadDepartments();
                return Page();
            }

            department.Name = Input.Name;
            department.Description = Input.Description;
            department.Performance = Input.Performance;
            department.DateCreated = Input.DateCreated;
            department.Budget = Input.Budget;
            department.Status = Input.Status;

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department != null)
            {
                // Check if there are employees in this department
                var hasEmployees = await _context.Employees.AnyAsync(e => e.DepartmentID == id);

                if (hasEmployees)
                {
                    TempData["ErrorMessage"] = "Cannot delete department with assigned employees. Please reassign employees first.";
                    return RedirectToPage();
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<JsonResult> OnGetDepartmentDetailsAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return new JsonResult(new { success = false, message = "Department not found." });
            }

            return new JsonResult(new
            {
                success = true,
                department = new
                {
                    department.DepartmentID,
                    department.Name,
                    department.Description,
                    department.Performance,
                    department.DateCreated,
                    department.Budget,
                    department.Status
                }
            });
        }

        public IActionResult OnPostLogout()
        {
            _logger.LogInformation("User logged out.");
            HttpContext.Session.Clear(); // Clear session
            return RedirectToPage("/Login");
        }
    }
}