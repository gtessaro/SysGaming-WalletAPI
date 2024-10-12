
using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/player")]
    public class PlayerController : ControllerBase
    {

        // private readonly AppDbContext _context;

        // public PlayerController(AppDbContext context)
        // {
        //     _context = context;
        // }

        [HttpPost]
        public async Task<Player> Create([FromBody]Player player)
        {
            // if (_context.Jogadores.Any(j => j.Email == jogador.Email))
            // {
            //     return BadRequest("E-mail já cadastrado.");
            // }

            // jogador.DataCriacao = DateTime.UtcNow;
            // _context.Jogadores.Add(jogador);
            // await _context.SaveChangesAsync();

            // return CreatedAtAction(nameof(GetJogador), new { id = jogador.Id }, jogador);
        
            return player;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(int id)
        {
            // var jogador = await _context.Jogadores.Include(j => j.Carteira)
            //                                       .FirstOrDefaultAsync(j => j.Id == id);
            // if (jogador == null)
            // {
            //     return NotFound();
            // }
            return Ok("jogador");
        }
    }
}