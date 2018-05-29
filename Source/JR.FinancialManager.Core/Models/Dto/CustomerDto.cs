using System;
using System.Collections.Generic;
using System.Text;

namespace JR.FinancialManager.Core.Models.Dto
{
    public class CustomerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double LastBalance { get; set; }
    }
}