using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JR.FinancialManager.Core.Data.Helpers;
using JR.FinancialManager.Core.Data.Repository.Definition;
using JR.FinancialManager.Core.Models;
using JR.FinancialManager.Core.Models.Dto;
using JR.FinancialManager.Core.Services.Definition;

namespace JR.FinancialManager.Core.Services.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ICustomerRepository customerRepository, ITransactionTypeRepository transactionTypeRepository,
            ITransactionRepository transactionRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<TransactionDto>> GetTransactions(ListRequestDto request)
        {
            var result = await _transactionRepository.GetPaged(request);

            return result;
        }

        public async Task<TransactionDto> CreateTransaction(TransactionDto p)
        {
            var result = await _transactionRepository.CreateWithRelated(p);

            var tran = _mapper.Map<TransactionDto>(result);

            return tran;
        }

        public async Task<TransactionDto> UpdateTransaction(TransactionDto p)
        {
            var result = await _transactionRepository.UpdateWithRelated(p);

            var tran = _mapper.Map<TransactionDto>(result);

            return tran;
        }

        public async Task<PagedResult<CustomerDto>> GetCustomers(ListRequestDto request)
        {
            var result = await _customerRepository.GetPaged(request);

            return result;
        }

        public async Task<PagedResult<TransactionTypeDto>> GetTransactionTypes(ListRequestDto request)
        {
            var result = await _transactionTypeRepository.GetPaged(request);

            return result;
        }
    }
}