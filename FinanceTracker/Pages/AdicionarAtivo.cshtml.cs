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
        
        public SelectList Tipos { get; set; }
        
        public IActionResult OnGet()
        {
            Tipos = new SelectList(new List<string> { "DepositoPrazo", "FundoInvestimento", "ImovelArrendado" });
            return Page();
        }
        
        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Ativo.Tipo))
            {
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

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