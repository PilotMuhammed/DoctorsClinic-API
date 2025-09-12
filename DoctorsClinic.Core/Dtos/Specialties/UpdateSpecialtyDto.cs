namespace DoctorsClinic.Core.Dtos.Specialties
{
    public class UpdateSpecialtyDto
    {
        public int SpecialtyID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}