namespace DoctorsClinic.Domain.Entities
{
    public class AccountStatus : BaseEntity<int>
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        public int FailedCount { get; set; }
        public string? Reason { get; set; }
        public DateTime? LockDateTime { get; set; }
    }
}