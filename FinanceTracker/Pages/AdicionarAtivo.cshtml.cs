using FinanceTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FinanceTracker.Pages
{
    public class AdicionarAtivoModel : PageModel
    {
        [BindProperty]
        public AtivoFinanceiro Ativo { get; set; } = new()
        {
            DataInicio = DateTime.Today
        };

        [BindProperty]
        public DepositoPrazo Deposito { get; set; } = new();

        [BindProperty]
        public FundoInvestimento Fundo { get; set; } = new();

        [BindProperty]
        public ImovelArrendado Imovel { get; set; } = new();

        // List of asset types (options)
        public SelectList Tipos { get; set; }

        // This method runs when the page loads
        public IActionResult OnGet()
        {
            // Populate the SelectList with the types of assets
            Tipos = new SelectList(new List<string> { "DepositoPrazo", "FundoInvestimento", "ImovelArrendado" });
            return Page();
        }

        // OnPost logic here as you have it
        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Ativo.Tipo))
            {
                // Reexibe a página se nenhum tipo for selecionado
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Atribuir dados específicos conforme o tipo do ativo
            switch (Ativo.Tipo)
            {
                case "DepositoPrazo":
                    Ativo.DepositoPrazo = Deposito;
                    break;
                case "FundoInvestimento":
                    Ativo.FundoInvestimento = Fundo;
                    break;
                case "ImovelArrendado":
                    Ativo.ImovelArrendado = Imovel;
                    break;
            }

            
            return RedirectToPage("/Home"); 
        }
    }
}
