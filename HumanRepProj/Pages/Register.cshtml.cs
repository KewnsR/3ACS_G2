using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HumanRepProj.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        private static List<User> users = new List<User>(); // Temporary in-memory storage

        public class RegisterInputModel
        {
            [Required, StringLength(50, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 50 characters.")]
            public string FullName { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
            public string Password { get; set; }

            [Required, Compare("Password", ErrorMessage = "Passwords do not match")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
            ErrorMessage = string.Empty;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (users.Exists(u => u.Email == Input.Email))
            {
                ErrorMessage = "Email is already registered!";
                return Page();
            }

            // Store user (Temporary, you should use a database)
            users.Add(new User { FullName = Input.FullName, Email = Input.Email, Password = Input.Password });

            return RedirectToPage("/Login"); // Redirect to login after successful registration
        }

        private class User
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
