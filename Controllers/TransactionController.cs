using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController: ControllerBase
    {
        
    [HttpGet("{playerId}")]
        public async Task<IActionResult> GetTransactions(int playerId)
        {
            // var transacoes = await _context.Transacoes
            //     .Where(t => t.JogadorId == jogadorId)
            //     .ToListAsync();

            // if (transacoes == null || !transacoes.Any())
            // {
            //     return NotFound("Nenhuma transação encontrada para o jogador.");
            // }

            return Ok("transacoes");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            // var jogador = await _context.Jogadores.FirstOrDefaultAsync(j => j.Id == transacao.JogadorId);
            // if (jogador == null)
            // {
            //     return BadRequest("Jogador não encontrado.");
            // }

            // transacao.DataHora = DateTime.UtcNow;
            // _context.Transacoes.Add(transacao);
            // await _context.SaveChangesAsync();

            // return CreatedAtAction(nameof(GetTransacao), new { id = transacao.Id }, transacao);
            return Ok(new Transaction());
        }

        [HttpGet("transaction/{id}")]
        public async Task<IActionResult> GetTransacao(int id)
        {
            // var transacao = await _context.Transacoes.FirstOrDefaultAsync(t => t.Id == id);
            // if (transacao == null)
            // {
            //     return NotFound();
            // }

            return Ok("transacao");
        }

    }
}