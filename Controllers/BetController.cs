
using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SysGaming_WalletAPI.Controllers
{    
    [ApiController]
    [Route("api/bet")]
    public class BetController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpPost]
        public async Task<IActionResult> CreateBet([FromBody] Bet bet)
        {
            var player = await _context.Players.Include(j => j.Wallet).FirstOrDefaultAsync(j => j.Id == bet.PlayerId);
            if (player == null || player.Wallet.Balance < bet.Value)
            {
                return BadRequest("Saldo insuficiente ou jogador não encontrado.");
            }

            bet.DateTime = DateTime.UtcNow;
            bet.Status = "Pendente";

            player.Wallet.Balance -= bet.Value;
            _context.Bets.Add(bet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBet), new { id = bet.Id }, bet);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBet(int id)
        {
            var bet = await _context.Bets.FirstOrDefaultAsync(a => a.Id == id);
            
            if (bet == null)
            {
                return NotFound();
            }

            return Ok(bet);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBet(int id)
        {
            var bet = await _context.Bets.FirstOrDefaultAsync(a => a.Id == id);
            if (bet == null || bet.Status == "Cancelada")
            {
                return BadRequest("Aposta não encontrada ou já cancelada.");
            }

            bet.Status = "Cancelada";
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}