// Pages/ViewEmployee.cshtml.cs
using HumanRepProj.Data;
using HumanRepProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HumanRepProj.Pages
{
    public class ViewEmployeeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ViewEmployeeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeID == id);

            if (Employee == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}