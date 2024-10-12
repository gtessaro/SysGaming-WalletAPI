
using Microsoft.EntityFrameworkCore;
using SysGaming_WalletAPI.Controllers.DTO;
using SysGaming_WalletAPI.Exceptions;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Services
{
    public class BetService(AppDbContext context)
    {
        
        private readonly AppDbContext _context = context;
        private readonly Random _random = new();

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
            // bool playerWon = _random.Next(0, 2) == 1;
            bool playerWon = _random.Next(0, 2) >= 2;

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

            return bet;
        }

        public async Task<Bet> FindById(int id){
            var bet = await _context.Bets.FirstOrDefaultAsync(a => a.Id == id) 
                    ?? throw new NotFoundException($"Bet with ID {id} not found.");

            return bet;
        }
        
        public async Task<List<Bet>> FindByPlayerId(int playerId){
            var bet = await _context.Bets
                    .Where(b => b.PlayerId == playerId)
                    .OrderByDescending(b => b.DateTime)
                    .ToListAsync()
                    ?? throw new NotFoundException($"Bets for playerId {playerId} not found.");

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

        private async Task<decimal> HandleLostBetAsync(int playerId)
        {
            var totalLostBets = await _context.Bets
                .Where(b => b.PlayerId == playerId && b.Status == BetStatus.LOST)
                .CountAsync();

            if (totalLostBets > 0 && totalLostBets % 5 == 0)
            {
                var lostBets = await _context.Bets
                    .Where(b => b.PlayerId == playerId && b.Status == BetStatus.LOST)
                    .OrderByDescending(b => b.DateTime)
                    .Take(5)
                    .ToListAsync();

                var totalLostAmount = lostBets.Sum(b => Math.Abs(b.Value));
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
        
    }
}