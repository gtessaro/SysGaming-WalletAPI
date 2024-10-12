
using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Controllers.DTO;
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

            if (_service.VerifyEmail(playerDTO.Email))
            {
                return BadRequest("E-mail já cadastrado.");
            }

            var player = await _service.SavePlayer(playerDTO);

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(int id)
        {
            var player = await _service.GetPlayerById(id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }
    }
}