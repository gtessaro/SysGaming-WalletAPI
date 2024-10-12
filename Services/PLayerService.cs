
using SysGaming_WalletAPI.Models;
using SysGaming_WalletAPI.Controllers.DTO;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace SysGaming_WalletAPI.Services
{
    public class PlayerService(AppDbContext context)
    {
        
        private readonly AppDbContext _context = context;

        public async Task<Player> SavePlayer(PlayerDTO playerDTO){
            
            Player player = ConvertFromPlayerDTO(playerDTO);

            player.CreatedDate = DateTime.UtcNow;
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            
            return player;
        }

        public bool VerifyEmail(string Email){
            return _context.Players.Any(j => j.Email == Email);
        }

        public async Task<PlayerDTO> GetPlayerById(int id){
            var player = await _context.Players.Include(j => j.Wallet)
                                                  .FirstOrDefaultAsync(j => j.Id == id);

            if (player == null)
            {
                return null;
            }
            return ConvertFromPlayer(player);
        }

        public Player ConvertFromPlayerDTO(PlayerDTO playerDTO){

            Player player = new()
            {
                Email = playerDTO.Email,
                Name = playerDTO.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(playerDTO.Password)
            };

            Wallet wallet = new()
            {
                Balance = playerDTO.WalletDTO.Balance,
                Currency = playerDTO.WalletDTO.Currency
            };

            player.Wallet = wallet;

            return player;
        }

        public PlayerDTO ConvertFromPlayer(Player player){

            WalletDTO walletDTO = new()
            {
                Balance = player.Wallet.Balance,
                Currency = player.Wallet.Currency
            };

            PlayerDTO playerDTO = new()
            {
                Email = player.Email,
                Name = player.Name,
                WalletDTO = walletDTO
            };

            return playerDTO;
        }
    }
}