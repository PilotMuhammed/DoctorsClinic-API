namespace DoctorsClinic.Core.Dtos.Account
{
    public class UsersCounter
    {
        public int UsersCount { get; set; }
        public int ActiveUsersCount { get; set; }
        public int InactiveUsersCount { get; set; }
        public int BlockedUsersCount { get; set; }
        public int UnBlockedUsersCount { get; set; }
    }
}