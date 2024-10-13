using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Services;
using SysGaming_WalletAPI.Controllers.DTO;
using SysGaming_WalletAPI.Exceptions;


namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController(TransactionService service) : ControllerBase
    {
        
        private readonly TransactionService _service = service;

        [HttpGet("transactions")]
        public async Task<IActionResult> GetPlayerBets(int playerId, int page = 1, int pageSize = 10)
        {
            var result = await _service.GetPlayerTransactionsAsync(playerId, page, pageSize);
            return Ok(result);
        }


        [HttpGet("transaction/{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            try
            {
                var transaction = await _service.GetTransactionById(id);

                return Ok(transaction);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Error.", Details = ex.Message });
            }
        }

    }
}