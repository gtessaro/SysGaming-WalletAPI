
using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Controllers.DTO;
using SysGaming_WalletAPI.Exceptions;
using SysGaming_WalletAPI.Services;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/player")]
    public class PlayerController(PlayerService service) : ControllerBase
    {

        // private readonly AppDbContext _context = context;
        private readonly PlayerService _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]PlayerDTO playerDTO)
        {
            try
            {
                var player = await _service.SavePlayer(playerDTO);

                return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
            }
            catch (DuplicateEmailException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Error.", Details = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(int id)
        {
            
            try
            {
                var player = await _service.GetPlayerById(id);
                return Ok(player);
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