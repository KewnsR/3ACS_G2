using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace HumanRepProj.Pages
{
    public class DashboardModel : PageModel
    {
        public string UserEmail { get; private set; } // Holds user email
        public string UserName { get; private set; }  // Holds user full name

        public IActionResult OnGet()
        {
            // Ensure session exists
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToPage("/Login");
            }

            // Get user session data
            UserEmail = HttpContext.Session.GetString("UserEmail");
            UserName = HttpContext.Session.GetString("UserName");

            return Page();
        }
    }
}
