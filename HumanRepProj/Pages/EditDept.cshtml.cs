using HumanRepProj.Data;
using HumanRepProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HumanRepProj.Pages
{
    public class EditDeptModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditDeptModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DepartmentInputModel Input { get; set; }

        public List<SelectListItem> Managers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var existingDepartment = await _context.Departments
                .AsTracking()
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(d => d.DepartmentID == id);

            if (existingDepartment == null)
            {
                return NotFound();
            }

            Input = new DepartmentInputModel
            {
                DepartmentID = existingDepartment.DepartmentID,
                Name = existingDepartment.Name,
                Description = existingDepartment.Description,
                Performance = existingDepartment.Performance,
                DateCreated = existingDepartment.DateCreated,
                Budget = existingDepartment.Budget,
                Status = existingDepartment.Status,
                ManagerID = existingDepartment.ManagerID
            };

            await PopulateDropdowns(existingDepartment.DepartmentID);
            return Page();
        }

        private async Task PopulateDropdowns(int departmentId)
        {
            Managers = await _context.Employees
                .Where(e => e.IsManager && e.Status == "Active")
                .Select(e => new SelectListItem
                {
                    Value = e.EmployeeID.ToString(),
                    Text = $"{e.FirstName} {e.LastName}",
                    Selected = e.EmployeeID == Input.ManagerID
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(Input.DepartmentID);
                return Page();
            }

            var department = await _context.Departments.FindAsync(Input.DepartmentID);
            if (department == null)
            {
                return NotFound();
            }

            // Update department properties
            department.Name = Input.Name;
            department.Description = Input.Description;
            department.Performance = Input.Performance;
            department.DateCreated = Input.DateCreated;
            department.Budget = Input.Budget;
            department.Status = Input.Status;
            department.ManagerID = Input.ManagerID;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/Departments");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving changes.");
                await PopulateDropdowns(Input.DepartmentID);
                return Page();
            }
        }

        public class DepartmentInputModel
        {
            public int DepartmentID { get; set; }

            [Required(ErrorMessage = "Department name is required.")]
            public string Name { get; set; }

            public string Description { get; set; }

            [Range(0, 100)]
            public decimal Performance { get; set; }

            [DataType(DataType.Date)]
            public DateTime DateCreated { get; set; }

            [Range(0, double.MaxValue)]
            public decimal Budget { get; set; }

            [Required]
            public string Status { get; set; }

            public int? ManagerID { get; set; }
        }
    }
}