using HumanRepProj.Data;
using HumanRepProj.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class EditDeptModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditDeptModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public DepartmentInputModel Input { get; set; }

    public List<Employee> Managers { get; set; } = new();



    public async Task<IActionResult> OnGetAsync(int id)
    {
        var existingEmployee = await _context.Employees
            .AsTracking()
            .Include(e => e.Department)
            .Include(e => e.Manager)
            .FirstOrDefaultAsync(e => e.EmployeeID == id);

        if (existingEmployee == null)
        {
            return NotFound();
        }


        await PopulateDropdowns();
        return Page();
    }

    private async Task PopulateDropdowns()
    {
        throw new NotImplementedException();
    }

    public class DepartmentInputModel
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Performance { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; }
        public int? ManagerID { get; set; }

        public List<SelectListItem> Managers { get; set; } = new();
    }

}
