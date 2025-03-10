using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HumanRepProj.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public class LoginInputModel
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, MinLength(6)]
            public string Password { get; set; }
        }
        public class RegisterModel : PageModel
        {
            [BindProperty]
            public RegisterInputModel Input { get; set; }

            public string ErrorMessage { get; set; }

            public void OnGet()
            {
            }

            public void OnPost()
            {
                if (!ModelState.IsValid)
                {
                    ErrorMessage = "Please correct the errors and try again.";
                    return;
                }

                // Add your registration logic here
            }

            public class RegisterInputModel
            {
                public string FullName { get; set; }
                public string Email { get; set; }
                public string Password { get; set; }
                public string ConfirmPassword { get; set; }
            }
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

            if (Input.Email == "admin@example.com" && Input.Password == "password")
            {
                // Store session for authentication
                HttpContext.Session.SetString("UserEmail", Input.Email);
                return RedirectToPage("/Dashboard");
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }
        }
    }
}
