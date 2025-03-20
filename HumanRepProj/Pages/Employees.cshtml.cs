using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HumanRepProj.Pages
{
    public class EmployeesModel : PageModel
    {
        private readonly ILogger<EmployeesModel> _logger;

        public EmployeesModel(ILogger<EmployeesModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var userEmail = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogWarning("Session expired or user not logged in.");
                return RedirectToPage("/Login");
            }

            _logger.LogInformation($"User {userEmail} accessed the Employees page.");
            return Page();
        }

        // ✅ Logout handler
        public IActionResult OnPostLogout()
        {
            _logger.LogInformation("User logged out.");
            HttpContext.Session.Clear(); // Clear session
            return RedirectToPage("/Login");
        }
    }
}
