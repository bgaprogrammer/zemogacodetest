using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;

namespace JR.FinancialManager.Core.Data.Helpers
{
    public static class PagedExtension
    {
        public static PagedResult<U> GetPaged<T, U>(this IQueryable<T> query, int page, int pageSize) where U : class
        {
            var result = new PagedResult<U>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.Total = query.Count();

            var pageCount = (double)result.Total / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Data = query.Skip(skip).Take(pageSize).ProjectTo<U>().ToList();

            return result;
        }
    }
}