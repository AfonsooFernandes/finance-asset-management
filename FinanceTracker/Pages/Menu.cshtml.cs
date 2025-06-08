using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FinanceTracker.Pages
{
    [Authorize]
    public class MenuModel : PageModel
    {
        private readonly ILogger<MenuModel> _logger;

        public MenuModel(ILogger<MenuModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Menu GET acessado.");
        }
    }
}