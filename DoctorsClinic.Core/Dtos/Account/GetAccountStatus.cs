namespace DoctorsClinic.Core.Dtos.Account
{
    public class GetAccountStatus
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
    }
}