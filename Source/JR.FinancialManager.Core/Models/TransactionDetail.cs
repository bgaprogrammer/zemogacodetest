using System;
using System.Collections.Generic;
using JR.FinancialManager.Core.Data.Repository.Definition;

namespace JR.FinancialManager.Core.Models
{
    public partial class TransactionDetail : IEntity
    {
        public long Id { get; set; }
        public double OldBalanceOrig { get; set; }
        public double NewBalanceOrig { get; set; }
        public double OldBalanceDest { get; set; }
        public double NewBalanceDest { get; set; }
        public bool IsFraud { get; set; }
        public bool IsFlaggedFraud { get; set; }

        public Transaction Transaction { get; set; }
    }
}