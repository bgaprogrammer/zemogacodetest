using System;
using System.Collections.Generic;
using JR.FinancialManager.Core.Data.Repository.Definition;

namespace JR.FinancialManager.Core.Models
{
    public partial class Transaction : IEntity
    {
        public long Id { get; set; }
        public int? Step { get; set; }
        public double Amount { get; set; }
        public long OriginCustomerId { get; set; }
        public long DestCustomerId { get; set; }
        public long TransactionTypeId { get; set; }
        public DateTime? ExecutionDate { get; set; }

        public Customer DestCustomer { get; set; }
        public Customer OriginCustomer { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
    }
}