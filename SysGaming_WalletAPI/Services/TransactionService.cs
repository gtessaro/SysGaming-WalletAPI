
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

        public async Task<PagedResult<Transaction>> GetPlayerTransactionsAsync(int playerId, int page, int pageSize)
        {
            var query = _context.Transactions
                .Where(b => b.PlayerId == playerId)
                .OrderByDescending(b => b.DateTime);

            // Total de apostas
            int totalItems = await query.CountAsync();

            // Registros paginados
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new Transaction
                {
                    Id = b.Id,
                    PlayerId = b.PlayerId,
                    Value = b.Value,
                    Type = b.Type,
                    DateTime = b.DateTime
                })
                .ToListAsync();

            return new PagedResult<Transaction>
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
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