using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos.Helper
{
    public class PaginationQuery : IPaginationQuery
    {
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }

    public class PaginationMetadata
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public PaginationMetadata(int totalCount, IPaginationQuery paginationQuery)
        {
            var pageSize = paginationQuery.PageSize ?? 10;
            var currentPage = paginationQuery.Page ?? 1;
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }

    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; }
        public PaginationMetadata Metadata { get; set; }

        public PaginatedResult(List<T> data, PaginationMetadata metadata)
        {
            Data = data;
            Metadata = metadata;
        }
    }
}
