using System;
using System.Collections.Generic;
using JR.FinancialManager.Core.Data.Repository.Definition;

namespace JR.FinancialManager.Core.Models
{
    public partial class Customer : IEntity
    {
        public Customer()
        {
            TransactionDestCustomer = new HashSet<Transaction>();
            TransactionOriginCustomer = new HashSet<Transaction>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<Transaction> TransactionDestCustomer { get; set; }
        public ICollection<Transaction> TransactionOriginCustomer { get; set; }
    }
}