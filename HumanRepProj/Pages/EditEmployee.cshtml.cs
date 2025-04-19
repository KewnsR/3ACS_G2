using HumanRepProj.Data;
using HumanRepProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanRepProj.Pages
{
    public class EditEmployeeModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditEmployeeModel> _logger;

        [BindProperty]
        public Employee Employee { get; set; }

        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Managers { get; set; }
        public List<SelectListItem> GenderOptions { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Male", Text = "Male" },
            new SelectListItem { Value = "Female", Text = "Female" },
            new SelectListItem { Value = "Other", Text = "Other" }
        };

        public List<SelectListItem> EmploymentTypeOptions { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Full-time", Text = "Full-time" },
            new SelectListItem { Value = "Part-time", Text = "Part-time" },
            new SelectListItem { Value = "Contract", Text = "Contract" }
        };

        public List<SelectListItem> StatusOptions { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Active", Text = "Active" },
            new SelectListItem { Value = "On Leave", Text = "On Leave" },
            new SelectListItem { Value = "Terminated", Text = "Terminated" }
        };

        public EditEmployeeModel(ApplicationDbContext context, ILogger<EditEmployeeModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.EmployeeID == id);

            if (Employee == null)
            {
                return NotFound();
            }

            await PopulateDropdowns();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors and try again.";
                await PopulateDropdowns();
                return Page();
            }

            _logger.LogInformation($"Attempting to update employee {Employee.EmployeeID}");

            var existingEmployee = await _context.Employees
                .AsTracking()
                .FirstOrDefaultAsync(e => e.EmployeeID == Employee.EmployeeID);

            if (existingEmployee == null)
            {
                TempData["ErrorMessage"] = "Employee not found";
                return Page();
            }

            existingEmployee.FirstName = Employee.FirstName;
            existingEmployee.LastName = Employee.LastName;
            existingEmployee.Email = Employee.Email;
            existingEmployee.PhoneNumber = Employee.PhoneNumber;
            existingEmployee.Address = Employee.Address;
            existingEmployee.DateOfBirth = Employee.DateOfBirth;
            existingEmployee.Gender = Employee.Gender;
            existingEmployee.DepartmentID = Employee.DepartmentID;
            existingEmployee.Position = Employee.Position;
            existingEmployee.Salary = Employee.Salary;
            existingEmployee.ManagerID = Employee.ManagerID;
            existingEmployee.EmploymentType = Employee.EmploymentType;
            existingEmployee.Status = Employee.Status;
            existingEmployee.IsManager = Employee.IsManager;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Employee updated successfully";
                return RedirectToPage("./ViewEmployee", new { id = Employee.EmployeeID });
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException != null
                    ? $"{ex.Message} Inner: {ex.InnerException.Message}"
                    : ex.Message;

                _logger.LogError($"Database error: {errorMessage}");
                TempData["ErrorMessage"] = $"Database error: {errorMessage}";
                await PopulateDropdowns();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError($"General error: {ex}");
                TempData["ErrorMessage"] = $"Error saving changes: {ex.Message}";
                await PopulateDropdowns();
                return Page();
            }
        }

        private async Task PopulateDropdowns()
        {
            Departments = await _context.Departments
                .OrderBy(d => d.Name)
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentID.ToString(),
                    Text = d.Name
                }).ToListAsync() ?? new List<SelectListItem>();

            Managers = await _context.Employees
                .Where(e => e.IsManager && e.EmployeeID != Employee.EmployeeID)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .Select(e => new SelectListItem
                {
                    Value = e.EmployeeID.ToString(),
                    Text = $"{e.LastName}, {e.FirstName} ({e.Position})"
                }).ToListAsync() ?? new List<SelectListItem>();
        }
    }
}