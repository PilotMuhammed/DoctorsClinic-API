namespace DoctorsClinic.Core.Dtos.Medicines
{
    public class UpdateMedicineDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
    }
}