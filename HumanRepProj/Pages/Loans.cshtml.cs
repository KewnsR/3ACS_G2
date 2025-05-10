using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using HumanRepProj.Data;

namespace HumanRepProj.Pages
{
    public class LoansModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoansModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Loans Loan { get; set; }

        public IList<Loans> LoansList { get; set; }

        public async Task OnGetAsync()
        {
            LoansList = await _context.Loans.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Set default values for new loans
            Loan.PaidLoan = 0; // New loans start with $0 paid
            Loan.LoanStatus = "Pending Approval"; // Admin sets status later

            _context.Loans.Add(Loan);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Loans"); // Refresh page
        }
    }
}