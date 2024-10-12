
using SysGaming_WalletAPI.Models;
using SysGaming_WalletAPI.Controllers.DTO;

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