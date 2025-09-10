namespace DoctorsClinic.Core.Dtos.Role
{
    public class CreateRole
    {
        public required string Name { get; set; }
        public required string NameAr { get; set; }
        public string? Description { get; set; }
        public required List<int> Permissions { get; set; }
    }
}