using System;
using System.Collections.Generic;
using System.Text;

namespace JR.FinancialManager.Core.Models.Dto
{
    public class ListRequestDto
    {
        public int take { get; set; }
        public int skip { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public List<ListSortDto> sort { get; set; }
        public MainFilterDto filter { get; set; }
    }

    public class ListSortDto
    {
        public string field { get; set; }
        public string dir { get; set; }
    }

    public class MainFilterDto
    {
        public List<FilterDto> filters { get; set; }
        public string logic { get; set; }
    }

    public class FilterDto
    {
        public string field { get; set; }
        public bool ignoreCase { get; set; }
        public string @operator { get; set; }
        public string value { get; set; }
    }
}