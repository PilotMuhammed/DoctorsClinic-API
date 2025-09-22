namespace DoctorsClinic.Core.Dtos.Doctors
{
    public class UpdateDoctorDto
    {
        public string? FullName { get; set; }
        public int? SpecialtyID { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public Guid? UserID { get; set; }
    }
}