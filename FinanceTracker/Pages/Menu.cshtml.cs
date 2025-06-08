using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace FinanceTracker.Pages
{
    public class MenuModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }

        // Método para realizar logout
        
    }
}