using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Data;
using FinanceTracker.Models;

namespace FinanceTracker.Controllers
{
    [ApiController]
    [Route("api/pagamentos")]
    public class PagamentoImpostosController : ControllerBase
    {
        private readonly FinanceTrackerContext _context;

        public PagamentoImpostosController(FinanceTrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagamentoImpostosDto>>> GetAll()
        {
            var pagamentos = await _context.PagamentoImpostos.ToListAsync();

            var dtoList = pagamentos.ConvertAll(p => new PagamentoImpostosDto
            {
                Id = p.Id,
                AtivoId = p.AtivoId,
                DataPagamento = p.DataPagamento,
                Valor = p.Valor
            });

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PagamentoImpostosDto>> GetById(int id)
        {
            var pagamento = await _context.PagamentoImpostos.FindAsync(id);

            if (pagamento == null)
                return NotFound("Pagamento não encontrado.");

            var dto = new PagamentoImpostosDto
            {
                Id = pagamento.Id,
                AtivoId = pagamento.AtivoId,
                DataPagamento = pagamento.DataPagamento,
                Valor = pagamento.Valor
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] PagamentoImpostosDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pagamento = new PagamentoImpostos
            {
                AtivoId = dto.AtivoId,
                DataPagamento = dto.DataPagamento,
                Valor = dto.Valor
            };

            _context.PagamentoImpostos.Add(pagamento);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pagamento registrado com sucesso." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] PagamentoImpostosDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pagamento = await _context.PagamentoImpostos.FindAsync(id);

            if (pagamento == null)
                return NotFound("Pagamento não encontrado.");

            pagamento.DataPagamento = dto.DataPagamento;
            pagamento.Valor = dto.Valor;
            pagamento.AtivoId = dto.AtivoId;

            _context.PagamentoImpostos.Update(pagamento);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pagamento atualizado com sucesso." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var pagamento = await _context.PagamentoImpostos.FindAsync(id);

            if (pagamento == null)
                return NotFound("Pagamento não encontrado.");

            _context.PagamentoImpostos.Remove(pagamento);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pagamento removido com sucesso." });
        }
    }
}