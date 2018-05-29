using System;
using System.Collections.Generic;
using JR.FinancialManager.Core.Data.Repository.Definition;

namespace JR.FinancialManager.Core.Models
{
    public partial class TransactionType : IEntity
    {
        public TransactionType()
        {
            Transaction = new HashSet<Transaction>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<Transaction> Transaction { get; set; }
    }
}