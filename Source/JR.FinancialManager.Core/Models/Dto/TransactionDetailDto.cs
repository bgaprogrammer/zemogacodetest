using System;
using System.Collections.Generic;
using System.Text;

namespace JR.FinancialManager.Core.Models.Dto
{
    public class TransactionDetailDto
    {
        public long Id { get; set; }
        public double OldBalanceOrig { get; set; }
        public double NewBalanceOrig { get; set; }
        public double OldBalanceDest { get; set; }
        public double NewBalanceDest { get; set; }
        public bool IsFraud { get; set; }
        public bool IsFlaggedFraud { get; set; }
    }
}