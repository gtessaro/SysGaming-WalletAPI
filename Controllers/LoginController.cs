
using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Controllers.DTO;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController: ControllerBase
    {
        [HttpPost]
        public async Task<Player> Login([FromBody]LoginDTO loginDTO)
        {

            // var jogador = await _context.Jogadores
            //     .Include(j => j.Carteira)
            //     .FirstOrDefaultAsync(j => j.Email == loginDto.Email && j.Senha == loginDto.Senha);

            // if (jogador == null)
            // {
            //     return Unauthorized("Credenciais inválidas.");
            // }


            Player player = new Player();
            player.Email = loginDTO.Email;
            player.Password = loginDTO.Password;
            return player;
        }
    }
}