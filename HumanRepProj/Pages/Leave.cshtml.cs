using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace HumanRepProj.Pages
{
    public class LeaveModel : PageModel
    {
        [BindProperty]
        public LeaveInput Input { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            // Initial form state
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Show validation errors
            }

            // Simulate storing the leave request
            Message = $"Leave request submitted for {Input.EmployeeName} from {Input.StartDate.ToShortDateString()} to {Input.EndDate.ToShortDateString()}.";

            ModelState.Clear(); // Clear validation messages
            Input = new LeaveInput(); // Reset the form

            return Page();
        }

        // ✅ Handles logout from the form
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            // Clear session and sign out
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();

            return RedirectToPage("/Login");
        }

        public class LeaveInput
        {
            [Required]
            public string EmployeeName { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime StartDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime EndDate { get; set; }

            [Required]
            public string Reason { get; set; }
        }
    }
}
