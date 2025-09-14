using DoctorsClinic.Core.Helper;

namespace DoctorsClinic.Core.Extensions
{
    public static class Pagination
    {
        public static IQueryable<T> ApplyPagging<T>(this IQueryable<T> query, IPaginationQuery queryObj)
        {
            if (!queryObj.Page.HasValue || !queryObj.PageSize.HasValue)
            {
                return query;
            }

            if (queryObj.Page.Value <= 0)
                queryObj.Page = 1;
            if (queryObj.PageSize.Value <= 1)
                queryObj.PageSize = 10;
            return query.Skip((queryObj.Page.Value - 1) * queryObj.PageSize.Value).Take(queryObj.PageSize.Value);
        }
    }
}