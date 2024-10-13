
using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Models;
using Microsoft.EntityFrameworkCore;
using SysGaming_WalletAPI.Controllers.DTO;
using SysGaming_WalletAPI.Services;
using SysGaming_WalletAPI.Exceptions;

namespace SysGaming_WalletAPI.Controllers
{    
    [ApiController]
    [Route("api/bet")]
    public class BetController(AppDbContext context, BetService service) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly BetService _service = service;

        [HttpPost]
        public async Task<IActionResult> CreateBet([FromBody] BetDTO betDTO)
        {
            try
            {
                var bet = await _service.CreateBet(betDTO);
            
                return CreatedAtAction(nameof(GetBet), new { id = bet.Id }, bet);
            }
            catch (InvalidOperationException ex )
            {
                return BadRequest(new { ex.Message });
            }
            catch (InsuficientBalanceException ex )
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Error.", Details = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBet(int id)
        {
            try
            {
                var bet = await _service.FindById(id);
            
                return Ok(bet);
            }
            catch (NotFoundException ex )
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Error.", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBet(int id)
        {

            try
            {
                await _service.CancelBet(id);

                return NoContent();
            }
            catch (NotFoundException ex )
            {
                return NotFound(new { ex.Message });
            }
            catch (InvalidOperationException ex )
            {
                return BadRequest(new { ex.Message });
            }
            catch (InsuficientBalanceException ex )
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Error.", Details = ex.Message });
            }
        }

        [HttpGet("bets")]
        public async Task<IActionResult> GetPlayerBets(int playerId, int page = 1, int pageSize = 10)
        {
            var result = await _service.GetPlayerBetsAsync(playerId, page, pageSize);
            return Ok(result);
        }
        
    }
}