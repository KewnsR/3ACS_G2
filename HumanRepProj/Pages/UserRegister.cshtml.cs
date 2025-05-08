using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HumanRepProj.Pages
{
    public class UserRegisterModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // Add this property to fix the error
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Initialize page
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Add your registration logic here
            // For example, checking if username already exists, saving to database, etc.

            // If registration is successful, redirect to login page
            return RedirectToPage("/UserLogin");

            // If there's an error during registration
            // ErrorMessage = "Username already exists. Please choose another.";
            // return Page();
        }
    }
}