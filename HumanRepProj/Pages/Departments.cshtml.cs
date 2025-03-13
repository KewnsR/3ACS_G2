using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanRepProj.Pages;

public class DepartmentsModel : PageModel
{
    private readonly ILogger<DepartmentsModel> _logger;

    public DepartmentsModel(ILogger<DepartmentsModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

