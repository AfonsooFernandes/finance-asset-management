using FinanceTracker.Models;
using FinanceTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FinanceTracker.Pages
{
    public class AdicionarDepositoModel : PageModel
    {
        private readonly AtivoFinanceiroService _ativoService;
        private readonly DepositoPrazoService _depositoService;

        public AdicionarDepositoModel(AtivoFinanceiroService ativoService, DepositoPrazoService depositoService)
        {
            _ativoService = ativoService;
            _depositoService = depositoService;
        }

        [BindProperty]
        public AtivoFinanceiroDto Ativo { get; set; } = new AtivoFinanceiroDto();

        [BindProperty]
        public DepositoPrazoDto Deposito { get; set; } = new DepositoPrazoDto();

        public void OnGet()
        {
            // Inicialização, se necessário
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Ativo.Tipo = "Depósito a Prazo";
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

            Console.WriteLine("DEPOSITO:");
            Console.WriteLine($"- Valor: {Deposito.Valor}");
            Console.WriteLine($"- Banco: {Deposito.Banco}");
            Console.WriteLine($"- TaxaJuro: {Deposito.TaxaJuroAnual}");
            Console.WriteLine($"- nuMERO CONTA: {Deposito.NumeroConta}");
            Console.WriteLine($"- titular: {Deposito.Titulares}");

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
            Deposito.AtivoId = ativoCriado.Id;

            var depositoResult = await _depositoService.CreateDeposito(Deposito);
            if (!depositoResult.ToLower().Contains("sucesso"))
            {
                ModelState.AddModelError(string.Empty, "Erro ao criar o depósito: " + depositoResult);
                return Page();
            }

            return RedirectToPage("/Menu");
        }
    }
}