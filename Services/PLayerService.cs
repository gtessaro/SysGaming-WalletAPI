
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

        public async Task<Player> GetPlayerById(int id){
            var player = await _context.Players.Include(j => j.Wallet)
                                                  .FirstOrDefaultAsync(j => j.Id == id);

            if (player == null)
            {
                return null;
            }
            return player;
        }

        public Player ConvertFromPlayerDTO(PlayerDTO playerDTO){

            Player player = new Player();
            player.Email = playerDTO.Email;
            player.Name = playerDTO.Name;
            player.Password = playerDTO.Password;

            Wallet wallet = new Wallet();
            wallet.Balance = playerDTO.WalletDTO.Balance;
            wallet.Currency = playerDTO.WalletDTO.Currency;

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
                Password = player.Password,
                WalletDTO = walletDTO
            };

            return playerDTO;
        }
    }
}