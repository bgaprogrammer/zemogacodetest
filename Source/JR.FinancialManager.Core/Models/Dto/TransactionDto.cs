using System;
using System.Collections.Generic;
using System.Text;

namespace JR.FinancialManager.Core.Models.Dto
{
    public class TransactionDto
    {
        public long Id { get; set; }
        public int? Step { get; set; }
        public double Amount { get; set; }
        public long OriginCustomerId { get; set; }
        public long DestCustomerId { get; set; }
        public long TransactionTypeId { get; set; }
        public DateTime? ExecutionDate { get; set; }

        public CustomerDto DestCustomer { get; set; }
        public CustomerDto OriginCustomer { get; set; }
        public TransactionTypeDto TransactionType { get; set; }
        public TransactionDetailDto TransactionDetail { get; set; }
    }
}