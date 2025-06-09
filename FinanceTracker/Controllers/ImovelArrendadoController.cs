using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Data;
using FinanceTracker.Models;

namespace FinanceTracker.Controllers
{
    [ApiController]
    [Route("api/imoveis")]
    public class ImovelArrendadoController : ControllerBase
    {
        private readonly FinanceTrackerContext _context;

        public ImovelArrendadoController(FinanceTrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImovelArrendadoDto>>> GetImoveis()
        {
            var imoveis = await _context.ImoveisArrendados.ToListAsync();

            var dtoList = imoveis.ConvertAll(i => new ImovelArrendadoDto
            {
                Id = i.Id,
                AtivoId = i.AtivoId,
                Designacao = i.Designacao,
                Localizacao = i.Localizacao,
                ValorImovel = i.ValorImovel,
                ValorRenda = i.ValorRenda,
                ValorCondominio = i.ValorCondominio,
                OutrasDespesas = i.OutrasDespesas
            });

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImovelArrendadoDto>> GetImovel(int id)
        {
            var imovel = await _context.ImoveisArrendados.FindAsync(id);

            if (imovel == null)
                return NotFound("Imóvel arrendado não encontrado.");

            var dto = new ImovelArrendadoDto
            {
                Id = imovel.Id,
                AtivoId = imovel.AtivoId,
                Designacao = imovel.Designacao,
                Localizacao = imovel.Localizacao,
                ValorImovel = imovel.ValorImovel,
                ValorRenda = imovel.ValorRenda,
                ValorCondominio = imovel.ValorCondominio,
                OutrasDespesas = imovel.OutrasDespesas
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateImovel([FromBody] ImovelArrendadoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imovel = new ImovelArrendado
            {
                AtivoId = dto.AtivoId,
                Designacao = dto.Designacao,
                Localizacao = dto.Localizacao,
                ValorImovel = dto.ValorImovel,
                ValorRenda = dto.ValorRenda,
                ValorCondominio = dto.ValorCondominio,
                OutrasDespesas = dto.OutrasDespesas
            };

            _context.ImoveisArrendados.Add(imovel);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Imóvel arrendado criado com sucesso." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateImovel(int id, [FromBody] ImovelArrendadoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imovel = await _context.ImoveisArrendados.FindAsync(id);

            if (imovel == null)
                return NotFound("Imóvel arrendado não encontrado.");

            imovel.Designacao = dto.Designacao;
            imovel.Localizacao = dto.Localizacao;
            imovel.ValorImovel = dto.ValorImovel;
            imovel.ValorRenda = dto.ValorRenda;
            imovel.ValorCondominio = dto.ValorCondominio;
            imovel.OutrasDespesas = dto.OutrasDespesas;

            _context.ImoveisArrendados.Update(imovel);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Imóvel arrendado atualizado com sucesso." });
        }
        
        [HttpGet("ativo/{ativoId}")]
        public async Task<ActionResult<ImovelArrendadoDto>> GetImovelByAtivoId(int ativoId)
        {
            var imovel = await _context.ImoveisArrendados
                .FirstOrDefaultAsync(i => i.AtivoId == ativoId);

            if (imovel == null)
                return NotFound("Imóvel arrendado não encontrado com o AtivoId fornecido.");

            var dto = new ImovelArrendadoDto
            {
                Id = imovel.Id,
                AtivoId = imovel.AtivoId,
                Designacao = imovel.Designacao,
                Localizacao = imovel.Localizacao,
                ValorImovel = imovel.ValorImovel,
                ValorRenda = imovel.ValorRenda,
                ValorCondominio = imovel.ValorCondominio,
                OutrasDespesas = imovel.OutrasDespesas
            };

            return Ok(dto);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteImovel(int id)
        {
            var imovel = await _context.ImoveisArrendados.FindAsync(id);

            if (imovel == null)
                return NotFound("Imóvel arrendado não encontrado.");

            _context.ImoveisArrendados.Remove(imovel);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Imóvel arrendado removido com sucesso." });
        }
    }
}