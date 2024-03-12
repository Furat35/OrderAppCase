namespace Shared.Entities.Common
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
