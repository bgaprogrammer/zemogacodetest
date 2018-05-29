using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JR.FinancialManager.Core.Data.Helpers;
using JR.FinancialManager.Core.Data.Repository.Definition;
using JR.FinancialManager.Core.Models;
using JR.FinancialManager.Core.Models.Dto;

namespace JR.FinancialManager.Core.Data.Repository.Implementation
{
    public class TransactionTypeRepository : GenericRepository<TransactionType>, ITransactionTypeRepository
    {
        private readonly FinancialManagerDbContext _dbContext;

        public TransactionTypeRepository(FinancialManagerDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<TransactionTypeDto>> GetPaged(ListRequestDto request)
        {
            PagedResult<TransactionTypeDto> data;

            if (request.filter.filters.Any())
            {
                var filter1 = request.filter.filters.FirstOrDefault();
                data = _dbContext.TransactionType.Where(x => x.Name.Contains(filter1.value)).GetPaged<TransactionType, TransactionTypeDto>(request.page, request.pageSize);
            }
            else
            {
                data = _dbContext.TransactionType.GetPaged<TransactionType, TransactionTypeDto>(request.page, request.pageSize);
            }

            return await Task.FromResult(data);
        }
    }
}