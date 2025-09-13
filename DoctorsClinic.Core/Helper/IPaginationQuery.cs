namespace DoctorsClinic.Core.Helper
{
    public interface IPaginationQuery
    {
        int? Page { get; set; }
        int? PageSize { get; set; }
    }
}
