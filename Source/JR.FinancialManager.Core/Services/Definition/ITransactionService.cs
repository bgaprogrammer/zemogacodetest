using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JR.FinancialManager.Core.Data.Helpers;
using JR.FinancialManager.Core.Models;
using JR.FinancialManager.Core.Models.Dto;

namespace JR.FinancialManager.Core.Services.Definition
{
    public interface ITransactionService
    {
        Task<PagedResult<CustomerDto>> GetCustomers(ListRequestDto request);
        Task<PagedResult<TransactionTypeDto>> GetTransactionTypes(ListRequestDto request);
        Task<TransactionDto> CreateTransaction(TransactionDto p);
        Task<PagedResult<TransactionDto>> GetTransactions(ListRequestDto request);
        Task<TransactionDto> UpdateTransaction(TransactionDto p);
    }
}