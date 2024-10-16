
using System.Net.WebSockets;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SysGaming_WalletAPI.Controllers.DTO;
using SysGaming_WalletAPI.Exceptions;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Services
{
    public class BetService(AppDbContext context,WebSocketConnectionManager webSocketManager)
    {
        
        private readonly AppDbContext _context = context;
        private readonly WebSocketConnectionManager _webSocketManager = webSocketManager;
        private readonly Random _random = new();

        public async Task<PagedResult<Bet>> GetPlayerBetsAsync(int playerId, int page, int pageSize)
        {
            var query = _context.Bets
                .Where(b => b.PlayerId == playerId)
                .OrderByDescending(b => b.DateTime);

            int totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new Bet
                {
                    Id = b.Id,
                    PlayerId = b.PlayerId,
                    Value = b.Value,
                    Status = b.Status,
                    DateTime = b.DateTime
                })
                .ToListAsync();

            return new PagedResult<Bet>
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
        }

        public async Task<Bet> CreateBet(BetDTO betDTO)
        {
            if (betDTO.Value < 1)
            {
                throw new InvalidOperationException("The minimun bet value is R$1,00 .");
            }

            var player = await _context.Players.Include(j => j.Wallet).FirstOrDefaultAsync(j => j.Id == betDTO.PlayerId);
            if (player == null || player.Wallet.Balance < betDTO.Value)
            {
                throw new InsuficientBalanceException("Insuficient Balance to do the BET");
            }else{
                player.Wallet.Balance -= betDTO.Value;
            }
            bool playerWon = _random.Next(0, 2) == 1;
            // bool playerWon = _random.Next(0, 2) >= 2;

            var bet = RegisterBet(betDTO, playerWon);

            RegisterTransaction(betDTO.PlayerId, TransactionType.BET, bet.Value);

            await _context.SaveChangesAsync();

            if (playerWon){
                player.Wallet.Balance += bet.Prize;
                RegisterTransaction(bet.PlayerId, TransactionType.PRIZE, bet.Prize);
            }else{
                player.Wallet.Balance += await HandleLostBetAsync(bet.PlayerId);
            }

            await _context.SaveChangesAsync();
            
            await NotifyWalletUpdate(player.Id, player.Wallet.Balance);

            return bet;
        }

        public async Task<Bet> FindById(int id){
            var bet = await _context.Bets.FirstOrDefaultAsync(a => a.Id == id) 
                    ?? throw new NotFoundException($"Bet with ID {id} not found.");

            return bet;
        }

        public async Task<bool> CancelBet(int id){
            var bet = await _context.Bets.FirstOrDefaultAsync(a => a.Id == id);

            if (bet == null || bet.Status == BetStatus.CANCELED)
            {
                throw new NotFoundException($"Bet with ID {id} not found.");
            }

            var player = await _context.Players.Include(j => j.Wallet).FirstOrDefaultAsync(j => j.Id == bet.PlayerId);
            if(bet.Status == BetStatus.WON){
                var valueToDeduce = bet.Prize - bet.Value;
                if(player.Wallet.Balance < valueToDeduce){
                    throw new InsuficientBalanceException("Insuficient Balance to CANCEL the BET");
                }else{
                    player.Wallet.Balance-=valueToDeduce;
                    RegisterTransaction(player.Id,TransactionType.CANCEL,valueToDeduce);
                }
            }
            if(bet.Status == BetStatus.LOST){
                player.Wallet.Balance+=bet.Value;
                RegisterTransaction(player.Id,TransactionType.CANCEL,bet.Value);
            }

            bet.Status = BetStatus.CANCELED;
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<decimal> HandleLostBetAsync(int playerId)
        {
            List<Bet> last5Bets = await _context.Bets
                    .Where(b => b.PlayerId == playerId )
                    .OrderByDescending(b => b.DateTime)
                    .Take(5)
                    .ToListAsync();
            
            if(last5Bets.Count<5){
                return 0;
            }

            bool allLost = true;

            foreach (var bet in last5Bets)
            {
                if(bet.Status == BetStatus.WON){
                    allLost = false;
                    break;
                }
            }

            if (allLost)
            {
                var totalLostAmount = last5Bets.Sum(b => Math.Abs(b.Value));
                var bonus = totalLostAmount * 0.10m; // Bônus de 10%

                RegisterTransaction(playerId, TransactionType.PRIZE, bonus);

                return bonus; 
            }

            return 0; 
        }


        private Bet RegisterBet(BetDTO betDTO, bool playerWon)
        {
            Bet bet = new()
            {
                DateTime = DateTime.UtcNow,
                Value = betDTO.Value,
                PlayerId = betDTO.PlayerId,
                Status = playerWon ? BetStatus.WON : BetStatus.LOST,
                Prize = playerWon ? betDTO.Value * 2 : 0
            };

            _context.Bets.Add(bet);
            return bet;
        }

        private void RegisterTransaction(int playerId,TransactionType type,decimal value){
            Transaction transaction = new()
            {
                PlayerId = playerId,
                Type = type,
                Value = value,
                DateTime = DateTime.UtcNow
            };
            _context.Transactions.Add(transaction);
        }

        private async Task NotifyWalletUpdate(int playerId, decimal newBalance)
        {
            var socket = _webSocketManager.GetConnection(playerId);
            if (socket != null && socket.State == WebSocketState.Open)
            {
                var message = Encoding.UTF8.GetBytes($"New Balance: {newBalance:C}");
                await socket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        
    }
}