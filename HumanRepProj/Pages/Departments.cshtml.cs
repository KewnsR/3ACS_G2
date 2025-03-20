using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HumanRepProj.Pages
{
    public class DepartmentsModel : PageModel
    {
        private readonly ILogger<DepartmentsModel> _logger;

        public DepartmentsModel(ILogger<DepartmentsModel> logger)
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

            _logger.LogInformation($"User {userEmail} accessed the Departments page.");
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
