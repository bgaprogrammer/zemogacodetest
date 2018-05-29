using System;
using System.Collections.Generic;
using System.Text;

namespace JR.FinancialManager.Core.Data.Helpers
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public List<T> Data { get; set; }

        public PagedResult()
        {
            Data = new List<T>();
        }
    }
}