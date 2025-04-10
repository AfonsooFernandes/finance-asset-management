using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace FinanceTracker.Pages
{
    public class ListarAtivosModel : PageModel
    {
        // Lista fictícia de ativos para simulação
        public List<AtivoFin> Ativos { get; set; }

        public ListarAtivosModel()
        {
            // Dados de exemplo - em um caso real, esses dados seriam recuperados do banco de dados
            Ativos = new List<AtivoFin>
            {
                new AtivoFin { Id = 1, Tipo = "DepositoPrazo", DataInicio = "2022-06-15", Duracao = 12, Imposto = 5.0f },
                new AtivoFin { Id = 2, Tipo = "FundoInvestimento", DataInicio = "2023-01-05", Duracao = 24, Imposto = 3.0f },
                new AtivoFin { Id = 3, Tipo = "ImovelArrendado", DataInicio = "2021-03-10", Duracao = 60, Imposto = 10.0f }
            };
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }

    // Classe fictícia para representar um ativo financeiro
    public class AtivoFin
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string DataInicio { get; set; }
        public int Duracao { get; set; }
        public float Imposto { get; set; }
    }
}