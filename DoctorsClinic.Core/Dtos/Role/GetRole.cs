using DoctorsClinic.Core.Dtos.Permission;

namespace DoctorsClinic.Core.Dtos.Role
{
    public class GetRole
    {
        public int Id { get; set; }   
        public string Name { get; set; } 
        public string NameAr { get; set; }
        public string? Description { get; set; }
        public List<GetEnum> Permissions { get; set; }
    }
}