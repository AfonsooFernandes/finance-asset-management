using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Data;
using FinanceTracker.Models;

namespace FinanceTracker.Controllers
{
    [ApiController]
    [Route("api/depositos")]
    public class DepositoPrazoController : ControllerBase
    {
        private readonly FinanceTrackerContext _context;

        public DepositoPrazoController(FinanceTrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepositoPrazoDto>>> GetDepositos()
        {
            var depositos = await _context.DepositosPrazo.ToListAsync();

            var dtoList = depositos.ConvertAll(d => new DepositoPrazoDto
            {
                Id = d.Id,
                AtivoId = d.AtivoId,
                Valor = d.Valor,
                Banco = d.Banco,
                NumeroConta = d.NumeroConta,
                Titulares = d.Titulares,
                TaxaJuroAnual = d.TaxaJuroAnual
            });

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepositoPrazoDto>> GetDeposito(int id)
        {
            var deposito = await _context.DepositosPrazo.FindAsync(id);

            if (deposito == null)
                return NotFound("Depósito de prazo não encontrado.");

            var dto = new DepositoPrazoDto
            {
                Id = deposito.Id,
                AtivoId = deposito.AtivoId,
                Valor = deposito.Valor,
                Banco = deposito.Banco,
                NumeroConta = deposito.NumeroConta,
                Titulares = deposito.Titulares,
                TaxaJuroAnual = deposito.TaxaJuroAnual
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateDeposito([FromBody] DepositoPrazoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deposito = new DepositoPrazo
            {
                AtivoId = dto.AtivoId,
                Valor = dto.Valor,
                Banco = dto.Banco,
                NumeroConta = dto.NumeroConta,
                Titulares = dto.Titulares,
                TaxaJuroAnual = dto.TaxaJuroAnual
            };

            _context.DepositosPrazo.Add(deposito);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Depósito de prazo criado com sucesso." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDeposito(int id, [FromBody] DepositoPrazoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deposito = await _context.DepositosPrazo.FindAsync(id);

            if (deposito == null)
                return NotFound("Depósito de prazo não encontrado.");

            deposito.Valor = dto.Valor;
            deposito.Banco = dto.Banco;
            deposito.NumeroConta = dto.NumeroConta;
            deposito.Titulares = dto.Titulares;
            deposito.TaxaJuroAnual = dto.TaxaJuroAnual;

            _context.DepositosPrazo.Update(deposito);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Depósito de prazo atualizado com sucesso." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeposito(int id)
        {
            var deposito = await _context.DepositosPrazo.FindAsync(id);

            if (deposito == null)
                return NotFound("Depósito de prazo não encontrado.");

            _context.DepositosPrazo.Remove(deposito);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Depósito de prazo removido com sucesso." });
        }
    }
}
