using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Models;
using Microsoft.EntityFrameworkCore;
using SysGaming_WalletAPI.Services;
using SysGaming_WalletAPI.Exceptions;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/wallet")]
    public class WalletController(AppDbContext context, WalletService service) : ControllerBase
    {

        private readonly AppDbContext _context = context;
        private readonly WalletService _service = service;

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetWallet(int playerId)
        {
            try
            {
                var wallet = await _service.FindByPlayerId(playerId);

                return Ok(wallet);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch(Exception ex){
                return StatusCode(500, new { Message = "Internal Error.", Details = ex.Message });
            }

        }

        [HttpPut("deposit/{WalletId}")]
        public async Task<IActionResult> Deposit(int WalletId, [FromBody] decimal value){

            try
            {
                await _service.Deposit(WalletId,value);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut("withdraw/{WalletId}")]
        public async Task<IActionResult> Withdraw(int WalletId, [FromBody] decimal value){

            try
            {
                await _service.Withdraw(WalletId,value);

                return NoContent();
            }
            catch (InsuficientBalanceException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}