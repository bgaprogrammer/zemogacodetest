using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JR.FinancialManager.Core.Data.Helpers;
using JR.FinancialManager.Core.Models;
using JR.FinancialManager.Core.Models.Dto;

namespace JR.FinancialManager.Core.Data.Repository.Definition
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<PagedResult<CustomerDto>> GetPaged(ListRequestDto request);
    }
}