using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JR.FinancialManager.Core.Data.Helpers;
using JR.FinancialManager.Core.Models;
using JR.FinancialManager.Core.Models.Dto;

namespace JR.FinancialManager.Core.Data.Repository.Definition
{
    public interface ITransactionTypeRepository : IGenericRepository<TransactionType>
    {
        Task<PagedResult<TransactionTypeDto>> GetPaged(ListRequestDto request);
    }
}