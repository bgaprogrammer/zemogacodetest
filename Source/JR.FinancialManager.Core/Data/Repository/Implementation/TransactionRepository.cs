using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JR.FinancialManager.Core.Data.Helpers;
using JR.FinancialManager.Core.Data.Repository.Definition;
using JR.FinancialManager.Core.Models;
using JR.FinancialManager.Core.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace JR.FinancialManager.Core.Data.Repository.Implementation
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly FinancialManagerDbContext _dbContext;

        public TransactionRepository(FinancialManagerDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<TransactionDto>> GetPaged(ListRequestDto request)
        {
            var data = _dbContext.Transaction.Include(x => x.TransactionType)
                                             .Include(x => x.DestCustomer)
                                             .Include(x => x.OriginCustomer)
                                             .Include(x => x.TransactionDetail)
                                             .GetPaged<Transaction, TransactionDto>(request.page, request.pageSize);

            return await Task.FromResult(data);
        }

        public async Task<Transaction> CreateWithRelated(TransactionDto t)
        {
            var tran = new Transaction
            {
                Amount = t.Amount,
                ExecutionDate = t.ExecutionDate,
                TransactionTypeId = t.TransactionType.Id,
                OriginCustomerId = t.OriginCustomer.Id,
                DestCustomerId = t.DestCustomer.Id,
                TransactionDetail = new TransactionDetail
                {
                    IsFraud = t.TransactionDetail.IsFraud,
                    IsFlaggedFraud = t.TransactionDetail.IsFlaggedFraud,
                    OldBalanceOrig = t.OriginCustomer.LastBalance,
                    NewBalanceOrig = (t.OriginCustomer.LastBalance - t.Amount),
                    OldBalanceDest = t.DestCustomer.LastBalance,
                    NewBalanceDest = (t.DestCustomer.LastBalance + t.Amount)
                }
            };

            await _dbContext.AddAsync(tran);
            await _dbContext.SaveChangesAsync();

            return _dbContext.Transaction.Include(x => x.TransactionType)
                                        .Include(x => x.DestCustomer)
                                        .Include(x => x.OriginCustomer)
                                        .Include(x => x.TransactionDetail)
                                        .FirstOrDefault(x => x.Id.Equals(tran.Id));
        }

        public async Task<Transaction> UpdateWithRelated(TransactionDto t)
        {
            var tran = await _dbContext.Transaction.Include(x => x.TransactionType)
                                                .Include(x => x.DestCustomer)
                                                .Include(x => x.OriginCustomer)
                                                .Include(x => x.TransactionDetail)
                                                .FirstOrDefaultAsync(x => x.Id.Equals(t.Id));

            if (tran != null)
            {
                tran.TransactionDetail.IsFraud = t.TransactionDetail.IsFraud;

                await _dbContext.SaveChangesAsync();
            }

            return tran;
        }
    }
}