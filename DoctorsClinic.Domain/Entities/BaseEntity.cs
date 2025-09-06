
namespace DoctorsClinic.Domain.Entities
{
    public class BaseEntity<T>
    {
        public T? Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? DeleterId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public Guid? ModifierId { get; set; }
        public DateTime? ModifieAt { get; set; }
    }
}