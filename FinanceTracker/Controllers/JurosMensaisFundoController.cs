using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Data;
using FinanceTracker.Models;

namespace FinanceTracker.Controllers
{
    [ApiController]
    [Route("api/juros")]
    public class JurosMensaisFundoController : ControllerBase
    {
        private readonly FinanceTrackerContext _context;

        public JurosMensaisFundoController(FinanceTrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JurosMensaisFundoDto>>> GetAll()
        {
            var juros = await _context.JurosMensaisFundos.ToListAsync();

            var dtoList = juros.ConvertAll(j => new JurosMensaisFundoDto
            {
                Id = j.Id,
                FundoId = j.FundoId,
                Mes = j.Mes,
                Ano = j.Ano,
                Taxa = j.Taxa
            });

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JurosMensaisFundoDto>> GetById(int id)
        {
            var juros = await _context.JurosMensaisFundos.FindAsync(id);

            if (juros == null)
                return NotFound("Registro de juros não encontrado.");

            var dto = new JurosMensaisFundoDto
            {
                Id = juros.Id,
                FundoId = juros.FundoId,
                Mes = juros.Mes,
                Ano = juros.Ano,
                Taxa = juros.Taxa
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] JurosMensaisFundoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var juros = new JurosMensaisFundo
            {
                FundoId = dto.FundoId,
                Mes = dto.Mes,
                Ano = dto.Ano,
                Taxa = dto.Taxa
            };

            _context.JurosMensaisFundos.Add(juros);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Juros mensais adicionados com sucesso." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] JurosMensaisFundoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var juros = await _context.JurosMensaisFundos.FindAsync(id);

            if (juros == null)
                return NotFound("Registro de juros não encontrado.");

            juros.Mes = dto.Mes;
            juros.Ano = dto.Ano;
            juros.Taxa = dto.Taxa;

            _context.JurosMensaisFundos.Update(juros);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Juros mensais atualizados com sucesso." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var juros = await _context.JurosMensaisFundos.FindAsync(id);

            if (juros == null)
                return NotFound("Registro de juros não encontrado.");

            _context.JurosMensaisFundos.Remove(juros);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Juros mensais removidos com sucesso." });
        }
    }
}
