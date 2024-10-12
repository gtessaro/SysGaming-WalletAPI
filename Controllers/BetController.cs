
using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Controllers
{    
    [ApiController]
    [Route("api/bet")]
    public class BetController : ControllerBase
    {
        // private readonly AppDbContext _context;

        // public ApostasController(AppDbContext context)
        // {
        //     _context = context;
        // }
        
        [HttpPost]
        public async Task<IActionResult> CreateBet([FromBody] Bet bet)
        {
            // var jogador = await _context.Jogadores.Include(j => j.Carteira).FirstOrDefaultAsync(j => j.Id == aposta.JogadorId);
            // if (jogador == null || jogador.Carteira.Saldo < aposta.Valor)
            // {
            //     return BadRequest("Saldo insuficiente ou jogador não encontrado.");
            // }

            // aposta.DataHora = DateTime.UtcNow;
            // aposta.Situacao = "Pendente";

            // jogador.Carteira.Saldo -= aposta.Valor;
            // _context.Apostas.Add(aposta);
            // await _context.SaveChangesAsync();

            // return CreatedAtAction(nameof(GetAposta), new { id = aposta.Id }, aposta);

            return Ok(bet);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBet(int id)
        {
            // var aposta = await _context.Apostas.FirstOrDefaultAsync(a => a.Id == id);
            // if (aposta == null)
            // {
            //     return NotFound();
            // }

            var bet = new Bet();
            bet.Id = id;

            return Ok(bet);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBet(int id)
        {
            // var aposta = await _context.Apostas.FirstOrDefaultAsync(a => a.Id == id);
            // if (aposta == null || aposta.Situacao == "Cancelada")
            // {
            //     return BadRequest("Aposta não encontrada ou já cancelada.");
            // }

            // aposta.Situacao = "Cancelada";
            // await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}