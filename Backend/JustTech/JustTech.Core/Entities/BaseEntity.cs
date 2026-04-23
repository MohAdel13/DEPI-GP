namespace JustTech.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        // Soft Delete Property
        public bool IsDeleted => DeletedAt.HasValue;
    }
}
