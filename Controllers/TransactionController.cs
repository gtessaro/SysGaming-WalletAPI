using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController(AppDbContext context) : ControllerBase
    {
        
        private readonly AppDbContext _context = context;

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetTransactions(int playerId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.PlayerId == playerId)
                .ToListAsync();

            if (transactions == null || !transactions.Any())
            {
                return NotFound("Nenhuma transação encontrada para o jogador.");
            }

            return Ok(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            var player = await _context.Players.FirstOrDefaultAsync(j => j.Id == transaction.PlayerId);
            if (player == null)
            {
                return BadRequest("Jogador não encontrado.");
            }

            transaction.DateTime = DateTime.UtcNow;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransacao), new { id = transaction.Id }, transaction);
        }

        [HttpGet("transaction/{id}")]
        public async Task<IActionResult> GetTransacao(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

    }
}