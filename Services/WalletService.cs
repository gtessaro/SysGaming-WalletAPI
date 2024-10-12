

using Microsoft.EntityFrameworkCore;
using SysGaming_WalletAPI.Exceptions;
using SysGaming_WalletAPI.Models;

namespace SysGaming_WalletAPI.Services
{
    public class WalletService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<Wallet> FindByPlayerId(int playerId){
            var wallet = await _context.Wallets.FirstOrDefaultAsync(c => c.PlayerId == playerId) 
                    ?? throw new NotFoundException($"Not Found any wallet for player {playerId}");

            return wallet;
        }
        
        public async Task<bool> Deposit(int WalletId, decimal value){
            var wallet = await _context.Wallets.FirstOrDefaultAsync(c => c.Id == WalletId) 
                    ?? throw new NotFoundException($"Wallet with id {WalletId} not found");
            wallet.Balance += value;
            RegisterTransaction(wallet.PlayerId,TransactionType.DEPOSIT,value);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Withdraw(int WalletId, decimal value){
            var wallet = await _context.Wallets.FirstOrDefaultAsync(c => c.Id == WalletId) 
                    ?? throw new NotFoundException($"Wallet with id {WalletId} not found");

            
            if(value > wallet.Balance){
                throw new InsuficientBalanceException($"Insuficient Balance for transaction {TransactionType.WITHDRAW} value {value}");
            }

            wallet.Balance -= value;
            RegisterTransaction(wallet.PlayerId,TransactionType.WITHDRAW,value);

            await _context.SaveChangesAsync();
            return true;
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