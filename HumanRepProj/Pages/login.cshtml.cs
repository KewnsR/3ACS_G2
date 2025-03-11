using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HumanRepProj.Data; // Ensure this namespace is correct
using HumanRepProj.Models; // Ensure this namespace is correct
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HumanRepProj.Pages // Ensure this namespace is correct
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnPost()
        {
            var user = _context.Logins.SingleOrDefault(u => u.Username == Username);
            if (user != null && VerifyPasswordHash(Password, user.PasswordHash))
            {
                // Set session or cookie here
                HttpContext.Session.SetString("Username", Username);
                return RedirectToPage("/Index");S
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString == storedHash;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
