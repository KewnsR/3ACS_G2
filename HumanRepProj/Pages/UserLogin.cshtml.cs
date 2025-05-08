using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HumanRepProj.Pages
{
    public class UserLoginModel : PageModel
    {
        [BindProperty]
        public LoginInputModel LoginInput { get; set; } = new LoginInputModel();

        [TempData]
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = null;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Authentication logic - replace with your actual implementation
            if (LoginInput.Username == "admin" && LoginInput.Password == "password")
            {
                // Simulate setting a session or authentication token
                HttpContext.Session.SetString("UserName", LoginInput.Username);

                // Redirect to UserDashboard after successful login
                return RedirectToPage("/UserDashboard");
            }

            ErrorMessage = "Invalid username or password";
            return Page();
        }
    }

    public class LoginInputModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
