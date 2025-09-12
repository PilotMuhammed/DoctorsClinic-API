namespace DoctorsClinic.Core.Dtos.Specialties
{
    public class SpecialtyDto
    {
        public int SpecialtyID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}