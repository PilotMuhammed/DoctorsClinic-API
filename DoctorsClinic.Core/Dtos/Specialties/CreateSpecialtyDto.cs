namespace DoctorsClinic.Core.Dtos.Specialties
{
    public class CreateSpecialtyDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}