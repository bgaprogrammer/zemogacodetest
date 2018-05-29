using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JR.FinancialManager.Core.Data.Helpers;
using JR.FinancialManager.Core.Models;
using JR.FinancialManager.Core.Models.Dto;

namespace JR.FinancialManager.Core.Data.Repository.Definition
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<PagedResult<TransactionDto>> GetPaged(ListRequestDto request);
        Task<Transaction> CreateWithRelated(TransactionDto p);
        Task<Transaction> UpdateWithRelated(TransactionDto p);
    }
}