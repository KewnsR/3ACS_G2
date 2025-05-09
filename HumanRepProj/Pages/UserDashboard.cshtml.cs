using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanRepProj.Pages
{
    public class UserDashboardModel : PageModel
    {
        public string UserName { get; private set; }
        public string FullName { get; private set; } = "John Doe";
        public string Role { get; private set; } = "Employee";

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                Response.Redirect("/UserLogin");
            }

            UserName = HttpContext.Session.GetString("UserName") ?? "User";
        }
    }
}