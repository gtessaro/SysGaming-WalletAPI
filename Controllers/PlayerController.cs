
using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/player")]
    public class PlayerController : ControllerBase
    {

        private readonly AppDbContext _context;

        public PlayerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Player player)
        {
            if (_context.Players.Any(j => j.Email == player.Email))
            {
                return BadRequest("E-mail já cadastrado.");
            }

            player.CreatedDate = DateTime.UtcNow;
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
        
            // return player;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(int id)
        {
            var player = await _context.Players.Include(j => j.Wallet)
                                                  .FirstOrDefaultAsync(j => j.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }
    }
}