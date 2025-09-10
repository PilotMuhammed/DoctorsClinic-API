namespace DoctorsClinic.Core.Dtos.Role
{
    public class UpdateRole
    {
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public string? Description { get; set; }
        public List<int>? Permissions { get; set; }
    }
}