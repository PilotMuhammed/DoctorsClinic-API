namespace DoctorsClinic.Core.Dtos.Doctors
{
    public class CreateDoctorDto
    {
        public required string FullName { get; set; }
        public int SpecialtyID { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public Guid UserID { get; set; }
    }
}