using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/wallet")]
    public class WalletController: ControllerBase
    {

        // private readonly AppDbContext _context;

        // public WalletController(AppDbContext context)
        // {
        //     _context = context;
        // }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetWallet(int playerId)
        {
            // var carteira = await _context.Carteiras.FirstOrDefaultAsync(c => c.JogadorId == jogadorId);
            // if (carteira == null)
            // {
            //     return NotFound();
            // }

            // return Ok(carteira);

            Wallet wallet = new Wallet();
            wallet.PlayerId = playerId;

            return Ok(wallet);
        }

        [HttpPut("update/{playerId}")]
        public async Task<IActionResult> UpdateBalance(int playerId, [FromBody] decimal value){

            // var carteira = await _context.Carteiras.FirstOrDefaultAsync(c => c.JogadorId == jogadorId);
            // if (carteira == null)
            // {
            //     return NotFound();
            // }

            // carteira.Saldo += valor;
            // await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}