using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using SysGaming_WalletAPI.Controllers.DTO;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Services
{
    public class LoginService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        public async Task<Player> DoLogin(LoginDTO loginDTO){
            var player = await _context.Players
                .Include(j => j.Wallet)
                .FirstOrDefaultAsync(j => j.Email == loginDTO.Email) ?? throw new InvalidCredentialException("Invalid credential");

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, player.Password);
            if(!isValidPassword){
                throw new InvalidCredentialException("Invalid credential");
            }
            return player;
        }
    }
}