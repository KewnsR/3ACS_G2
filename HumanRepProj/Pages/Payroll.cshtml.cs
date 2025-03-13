using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanRepProj.Pages;

public class PayrollModel : PageModel
{
    private readonly ILogger<PayrollModel> _logger;

    public PayrollModel(ILogger<PayrollModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

