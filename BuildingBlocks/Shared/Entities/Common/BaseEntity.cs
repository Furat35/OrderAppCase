namespace Shared.Entities.Common
{
    public class BaseEntity : IBaseEntity
    {
        public virtual Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
