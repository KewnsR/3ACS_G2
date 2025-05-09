using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanRepProj.Pages
{
    public class UserProfileModel : PageModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public void OnGet()
        {
            // Simulate fetching user data (replace with actual data retrieval logic)
            UserName = "johndoe";
            FullName = "John Doe";
            Email = "johndoe@example.com";
            Role = "Standard User";
        }
    }
}