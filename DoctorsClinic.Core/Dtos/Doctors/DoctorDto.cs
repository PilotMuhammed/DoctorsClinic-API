namespace DoctorsClinic.Core.Dtos.Doctors
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public int SpecialtyID { get; set; }
        public string? SpecialtyName { get; set; } 
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public Guid UserID { get; set; }
        public string? UserName { get; set; } 
    }
}