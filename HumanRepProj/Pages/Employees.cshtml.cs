using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanRepProj.Pages;

public class EmployeesModel : PageModel
{
    private readonly ILogger<EmployeesModel> _logger;

    public EmployeesModel(ILogger<EmployeesModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
