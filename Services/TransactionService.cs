
using SysGaming_WalletAPI.Models;
using SysGaming_WalletAPI.Controllers.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SysGaming_WalletAPI.Exceptions;


namespace SysGaming_WalletAPI.Services
{
    public class TransactionService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<List<TransactionDTO>> GetTransactionsByPlayer(int playerId){
            var transactions = await _context.Transactions
                .Where(t => t.PlayerId == playerId)
                .ToListAsync();

            if (transactions == null || !transactions.Any())
            {
                throw new NotFoundException($"No transactions for player {playerId}.");
            }

            var transactionDTOs = transactions.Select(t => new TransactionDTO
            {
                PlayerId = t.PlayerId,
                Type = t.Type, 
                Value = t.Value,
                DateTime = t.DateTime
            }).ToList();

            return transactionDTOs;
        }

        public async Task<Transaction> CreateTransaction(TransactionDTO transactionDTO){
            var player = await _context.Players.FirstOrDefaultAsync(j => j.Id == transactionDTO.PlayerId);
            if (player == null)
            {
                throw new NotFoundException($"Player with ID {transactionDTO.PlayerId} not found.");
            }
            
            //colocar no wallet Service
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.PlayerId == player.Id);
            if(wallet == null){
                throw new NotFoundException($"Player with ID {transactionDTO.PlayerId} has no wallet.");
            }
            // Ex.: "DEPOSIT", "BET", "PRIZE", "CANCEL","WITHDRAW"
            if(transactionDTO.Type == TransactionType.DEPOSIT ||
                transactionDTO.Type == TransactionType.PRIZE ){
                    wallet.Balance = transactionDTO.Value + wallet.Balance;
            }
            if(transactionDTO.Type == TransactionType.BET ||
                transactionDTO.Type == TransactionType.WITHDRAW ){
                    if(transactionDTO.Value > wallet.Balance){
                        throw new InsuficientBalanceException($"Insuficient Balance for transaction {transactionDTO.Type} value {transactionDTO.Value}");
                    }
                    wallet.Balance = transactionDTO.Value - wallet.Balance;
            }
            
            var transaction = FromDTO(transactionDTO);

            transaction.DateTime = DateTime.UtcNow;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            
            return transaction;
        }

        public async Task<Transaction> GetTransactionById(int id){
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                throw new NotFoundException($"No transaction with id {id}.");
            }

            return transaction;
        }

        public Transaction FromDTO(TransactionDTO transactionDTO){

            Transaction transaction = new()
            {
                    PlayerId = transactionDTO.PlayerId,
                    DateTime = transactionDTO.DateTime,
                    Type = transactionDTO.Type,
                    Value = transactionDTO.Value
            };

            return transaction;
        }

        public TransactionDTO ToDTO(Transaction transaction){

            TransactionDTO transactionDTO = new()
            {
                    PlayerId = transaction.PlayerId,
                    DateTime = transaction.DateTime,
                    Type = transaction.Type,
                    Value = transaction.Value
            };

            return transactionDTO;
        }

    }
}