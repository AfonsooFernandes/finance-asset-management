using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Data;
using FinanceTracker.Models;

namespace FinanceTracker.Controllers
{
    [ApiController]
    [Route("api/fundos")]
    public class FundoInvestimentoController : ControllerBase
    {
        private readonly FinanceTrackerContext _context;

        public FundoInvestimentoController(FinanceTrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FundoInvestimentoDto>>> GetFundos()
        {
            var fundos = await _context.FundosInvestimento.ToListAsync();

            var dtoList = fundos.ConvertAll(f => new FundoInvestimentoDto
            {
                Id = f.Id,
                AtivoId = f.AtivoId,
                Nome = f.Nome,
                Montante = f.Montante,
                TaxaJuro = f.TaxaJuro
            });

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FundoInvestimentoDto>> GetFundo(int id)
        {
            var fundo = await _context.FundosInvestimento.FindAsync(id);

            if (fundo == null)
                return NotFound("Fundo de investimento não encontrado.");

            var dto = new FundoInvestimentoDto
            {
                Id = fundo.Id,
                AtivoId = fundo.AtivoId,
                Nome = fundo.Nome,
                Montante = fundo.Montante,
                TaxaJuro = fundo.TaxaJuro
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFundo([FromBody] FundoInvestimentoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fundo = new FundoInvestimento
            {
                AtivoId = dto.AtivoId,
                Nome = dto.Nome,
                Montante = dto.Montante,
                TaxaJuro = dto.TaxaJuro
            };

            _context.FundosInvestimento.Add(fundo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Fundo de investimento criado com sucesso." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFundo(int id, [FromBody] FundoInvestimentoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fundo = await _context.FundosInvestimento.FindAsync(id);

            if (fundo == null)
                return NotFound("Fundo de investimento não encontrado.");

            fundo.Nome = dto.Nome;
            fundo.Montante = dto.Montante;
            fundo.TaxaJuro = dto.TaxaJuro;

            _context.FundosInvestimento.Update(fundo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Fundo de investimento atualizado com sucesso." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFundo(int id)
        {
            var fundo = await _context.FundosInvestimento.FindAsync(id);

            if (fundo == null)
                return NotFound("Fundo de investimento não encontrado.");

            _context.FundosInvestimento.Remove(fundo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Fundo de investimento removido com sucesso." });
        }
    }
}