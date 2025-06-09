using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FinanceTracker.Data;
using FinanceTracker.Models;
using System;

namespace FinanceTracker.Controllers
{
    [ApiController]
    [Route("api/ativos")]
    public class AtivosFinanceirosController : ControllerBase
    {
        private readonly FinanceTrackerContext _context;

        public AtivosFinanceirosController(FinanceTrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AtivoFinanceiroDto>>> GetAtivos()
        {
            var ativos = await _context.AtivosFinanceiros.ToListAsync();

            var ativosDto = ativos.ConvertAll(a => new AtivoFinanceiroDto
            {
                Id = a.Id,
                UtilizadorId = a.UtilizadorId,
                Tipo = a.Tipo,
                DataInicio = a.DataInicio,
                Duracao = a.Duracao,
                Imposto = a.Imposto
            });

            return Ok(ativosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AtivoFinanceiroDto>> GetAtivo(int id)
        {
            var ativo = await _context.AtivosFinanceiros.FindAsync(id);

            if (ativo == null)
                return NotFound("Ativo financeiro não encontrado.");

            var dto = new AtivoFinanceiroDto
            {
                Id = ativo.Id,
                UtilizadorId = ativo.UtilizadorId,
                Tipo = ativo.Tipo,
                DataInicio = ativo.DataInicio,
                Duracao = ativo.Duracao,
                Imposto = ativo.Imposto
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<AtivoFinanceiroDto>> CreateAtivo([FromBody] AtivoFinanceiroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ativo = new AtivoFinanceiro
            {
                UtilizadorId = dto.UtilizadorId,
                Tipo = dto.Tipo,
                DataInicio = DateTime.SpecifyKind(dto.DataInicio, DateTimeKind.Utc),
                Duracao = dto.Duracao,
                Imposto = dto.Imposto
            };

            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();

            dto.Id = ativo.Id;

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAtivo(int id, [FromBody] AtivoFinanceiroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ativo = await _context.AtivosFinanceiros.FindAsync(id);

            if (ativo == null)
                return NotFound("Ativo financeiro não encontrado.");

            ativo.Tipo = dto.Tipo;
            ativo.DataInicio = DateTime.SpecifyKind(dto.DataInicio, DateTimeKind.Utc);
            ativo.Duracao = dto.Duracao;
            ativo.Imposto = dto.Imposto;

            _context.AtivosFinanceiros.Update(ativo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Ativo financeiro atualizado com sucesso." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAtivo(int id)
        {
            var ativo = await _context.AtivosFinanceiros.FindAsync(id);

            if (ativo == null)
                return NotFound("Ativo financeiro não encontrado.");

            _context.AtivosFinanceiros.Remove(ativo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Ativo financeiro removido com sucesso." });
        }

        [HttpGet("usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<AtivoFinanceiroDto>>> GetAtivosByUserId(int userId)
        {
            var ativos = await _context.AtivosFinanceiros
                .Where(a => a.UtilizadorId == userId)
                .ToListAsync();

            var ativosDto = ativos.ConvertAll(a => new AtivoFinanceiroDto
            {
                Id = a.Id,
                UtilizadorId = a.UtilizadorId,
                Tipo = a.Tipo,
                DataInicio = a.DataInicio,
                Duracao = a.Duracao,
                Imposto = a.Imposto
            });

            return Ok(ativosDto);
        }
    }
}
