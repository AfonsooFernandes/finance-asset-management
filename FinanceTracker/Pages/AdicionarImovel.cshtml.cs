using FinanceTracker.Models;
using FinanceTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FinanceTracker.Pages
{
    public class AdicionarImovelModel : PageModel
    {
        private readonly AtivoFinanceiroService _ativoService;
        private readonly ImovelArrendadoService _imovelService;

        public AdicionarImovelModel(AtivoFinanceiroService ativoService, ImovelArrendadoService imovelService)
        {
            _ativoService = ativoService;
            _imovelService = imovelService;
        }

        [BindProperty]
        public AtivoFinanceiroDto Ativo { get; set; } = new AtivoFinanceiroDto();

        [BindProperty]
        public ImovelArrendadoDto Imovel { get; set; } = new ImovelArrendadoDto();

        public void OnGet()
        {
            // Inicialização, se necessário
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Ativo.Tipo = "Imóvel Arrendado";
            Ativo.DataInicio = DateTime.SpecifyKind(Ativo.DataInicio, DateTimeKind.Utc);
            
            // Debug: imprimir os valores recebidos
            Console.WriteLine("========= RECEBIDO NO POST =========");

            Console.WriteLine("ATIVO:");
            Console.WriteLine($"- Id: {Ativo.Id}");
            Console.WriteLine($"- IdUser: {Ativo.UtilizadorId}");
            Console.WriteLine($"- DataInicio: {Ativo.DataInicio}");
            Console.WriteLine($"- Tipo: {Ativo.Tipo}");
            Console.WriteLine($"- Duracao: {Ativo.Duracao}");
            Console.WriteLine($"- Imposto: {Ativo.Imposto}");

            Console.WriteLine("IMOVEL:");
            Console.WriteLine($"- Designacao: {Imovel.Designacao}");
            Console.WriteLine($"- Localizacao: {Imovel.Localizacao}");
            Console.WriteLine($"- ValorImovel: {Imovel.ValorImovel}");
            Console.WriteLine($"- ValorRenda: {Imovel.ValorRenda}");
            Console.WriteLine($"- ValorCondominio: {Imovel.ValorCondominio}");
            Console.WriteLine($"- OutrasDespesas: {Imovel.OutrasDespesas}");

            Console.WriteLine("ModelState válido? " + ModelState.IsValid);
            foreach (var kvp in ModelState)
            {
                foreach (var error in kvp.Value.Errors)
                {
                    Console.WriteLine($"ModelState Error ({kvp.Key}): {error.ErrorMessage}");
                }
            }

            Console.WriteLine("====================================");
            
            if (!ModelState.IsValid)
                return Page();

            var ativoCriado = await _ativoService.CreateAtivo(Ativo);

            if (ativoCriado == null || ativoCriado.Id == 0)
            {
                ModelState.AddModelError(string.Empty, "Erro ao criar o ativo financeiro.");
                return Page();
            }
            Console.WriteLine($"- Ativo: {ativoCriado.Id}");
            Imovel.AtivoId = ativoCriado.Id;

            var imovelResult = await _imovelService.CreateImovel(Imovel);
            if (!imovelResult.ToLower().Contains("sucesso"))
            {
                ModelState.AddModelError(string.Empty, "Erro ao criar o depósito: " + imovelResult);
                return Page();
            }

            return RedirectToPage("/Menu");
        }
    }
}