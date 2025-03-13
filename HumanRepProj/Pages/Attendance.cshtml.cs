using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanRepProj.Pages;

public class AttendanceModel : PageModel
{
    private readonly ILogger<AttendanceModel> _logger;

    public AttendanceModel(ILogger<AttendanceModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

