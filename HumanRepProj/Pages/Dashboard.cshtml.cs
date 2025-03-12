using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HumanRepProj.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ILogger<DashboardModel> _logger;

        public string UserEmail { get; private set; } // Holds user email
        public string UserName { get; private set; }  // Holds user full name

        public DashboardModel(ILogger<DashboardModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // ✅ Use "Username" for session consistency
            UserEmail = HttpContext.Session.GetString("Username");
            UserName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(UserEmail))
            {
                _logger.LogWarning("Session expired or user not logged in.");
                return RedirectToPage("/Login");
            }

            _logger.LogInformation($"User {UserEmail} logged into the dashboard.");

            return Page();
        }
    }
}
