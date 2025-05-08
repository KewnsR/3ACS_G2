using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanRepProj.Pages
{
    public class UserDashboardModel : PageModel
    {
        public string UserName { get; private set; }

        public void OnGet()
        {
            // Simulate user authentication check
            if (HttpContext.Session.GetString("UserName") == null)
            {
                Response.Redirect("/UserLogin");
            }

            UserName = HttpContext.Session.GetString("UserName");
        }
    }
}
