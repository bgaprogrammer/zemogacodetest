using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly FinancialManagerDbContext _dbContext;

        public CustomerRepository(FinancialManagerDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<CustomerDto>> GetPaged(ListRequestDto request)
        {
            PagedResult<CustomerDto> data;

            if (request.filter.filters.Any())
            {
                var filter1 = request.filter.filters.FirstOrDefault();
                data = _dbContext.Customer.Where(x => x.Name.StartsWith(filter1.value)).GetPaged<Customer, CustomerDto>(request.page, request.pageSize);
            }
            else
            {
                data = _dbContext.Customer.GetPaged<Customer, CustomerDto>(request.page, request.pageSize);
            }

            //Use a SPROC to get the last balance
            var paramIn = new SqlParameter
            {
                ParameterName = "userId",
                DbType = System.Data.DbType.Int64,
                Direction = System.Data.ParameterDirection.Input
            };

            var paramOut = new SqlParameter
            {
                ParameterName = "result",
                DbType = System.Data.DbType.Double,
                Direction = System.Data.ParameterDirection.Output
            };

            data.Data.ForEach(x =>
            {
                paramIn.Value = x.Id;

                _dbContext.Database.ExecuteSqlCommand("[dbo].[usp_GetCustomerLastBalance] @userId, @result OUT", paramIn, paramOut);

                x.LastBalance = (double) paramOut.Value;
            });

            return await Task.FromResult(data);
        }
    }
}