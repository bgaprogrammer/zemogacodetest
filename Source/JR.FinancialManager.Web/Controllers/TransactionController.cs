using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JR.FinancialManager.Core.Models;
using JR.FinancialManager.Core.Models.Dto;
using JR.FinancialManager.Core.Services.Definition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JR.FinancialManager.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("list")]
        public async Task<IActionResult> ListTransactionsAsync([FromBody] ListRequestDto request)
        {
            var result = await _transactionService.GetTransactions(request);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] TransactionDto t)
        {
            var result = await _transactionService.CreateTransaction(t);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransactionAsync([FromBody] TransactionDto t)
        {
            var result = await _transactionService.UpdateTransaction(t);

            return Ok(result);
        }

        [HttpPost("customer")]
        public async Task<IActionResult> ListCustomersAsync([FromBody] ListRequestDto request)
        {
            var result = await _transactionService.GetCustomers(request);

            return Ok(result);
        }

        [HttpPost("type")]
        public async Task<IActionResult> ListTransactionTypesAsync([FromBody] ListRequestDto request)
        {
            var result = await _transactionService.GetTransactionTypes(request);

            return Ok(result);
        }
    }
}