using DoctorsClinic.Core.Dtos.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos
{
    public class PaginationDto<T>
    {
        public List<T> Data { get; set; }
        public decimal TotalAmount { get; set; }
        public PaginationMetadata Meta { get; set; }

        public PaginationDto(List<T> data, decimal totalAmount, PaginationMetadata meta)
        {
            Data = data;
            TotalAmount = totalAmount;
            Meta = meta;
        }
        public PaginationDto(List<T> data, PaginationMetadata meta)
        {
            Data = data;
            Meta = meta;
        }
    }
}
