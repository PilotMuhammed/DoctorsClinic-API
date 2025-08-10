using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorsClinic.Core.Helper;

namespace DoctorsClinic.Core.Extensions
{
    public static class Pagination
    {
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 200;

        public static IQueryable<T> ApplyPagging<T>(this IQueryable<T> query, IPaginationQuery queryObj)
        {
            if (queryObj is null)
                return query;

            if (!queryObj.Page.HasValue || !queryObj.PageSize.HasValue)
                return query;

            if (queryObj.Page.Value <= 0)
                queryObj.Page = DefaultPage;

            if (queryObj.PageSize.Value < 1)
                queryObj.PageSize = DefaultPageSize;

            if (queryObj.PageSize.Value > MaxPageSize)
                queryObj.PageSize = MaxPageSize;

            return query
                .Skip((queryObj.Page.Value - 1) * queryObj.PageSize.Value)
                .Take(queryObj.PageSize.Value);
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IPaginationQuery queryObj)
            => ApplyPagging(query, queryObj);
    }
}