using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/wallet")]
    public class WalletController: ControllerBase
    {

        private readonly AppDbContext _context;

        public WalletController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetWallet(int playerId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(c => c.PlayerId == playerId);
            if (wallet == null)
            {
                return NotFound();
            }

            return Ok(wallet);

        }

        [HttpPut("update/{playerId}")]
        public async Task<IActionResult> UpdateBalance(int playerId, [FromBody] decimal value){

            var wallet = await _context.Wallets.FirstOrDefaultAsync(c => c.PlayerId == playerId);
            if (wallet == null)
            {
                return NotFound();
            }

            wallet.Balance += value;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}