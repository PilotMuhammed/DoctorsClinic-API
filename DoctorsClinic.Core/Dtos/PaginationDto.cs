using DoctorsClinic.Core.Helper;

namespace DoctorsClinic.Core.Dtos
{
    public class PaginationDto<T>
    {
        public List<T> Data { get; set; }
        public decimal TotalAmount { get; set; }
        public PaginationMetadata Meta { get; set; }

        public PaginationDto(List<T> data, decimal totalAmount, PaginationMetadata metadata)
        {
            Data = data;
            TotalAmount = totalAmount;
            Meta = metadata;
        }
        public PaginationDto(List<T> data, PaginationMetadata metadata)
        {
            Data = data;
            Meta = metadata;
        }
    }
}