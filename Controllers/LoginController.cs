using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SysGaming_WalletAPI.Controllers.DTO;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController: ControllerBase
    {

        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDTO)
        {

            var player = await _context.Players
                .Include(j => j.Wallet)
                .FirstOrDefaultAsync(j => j.Email == loginDTO.Email && j.Password == loginDTO.Password);

            if (player == null)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            return Ok(player);
        }
    }
}