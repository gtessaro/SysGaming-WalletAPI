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

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetTransactions(int playerId)
        {
          
            try
            {
                var transactions = await _service.GetTransactionsByPlayer(playerId);

                return Ok(transactions);
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

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDTO transactionDTO)
        {
            try
            {
                var transaction = await _service.CreateTransaction(transactionDTO);
                return CreatedAtAction(nameof(GetTransactions), new { id = transaction.Id }, transaction);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (InsuficientBalanceException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Error.", Details = ex.Message });
            }
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