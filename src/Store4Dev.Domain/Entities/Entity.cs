namespace Store4Dev.Domain.Entities
{
    public class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; protected set; }

        public DateTimeOffset CreatedAt { get; protected set; }

        public DateTimeOffset? UpdatedAt { get; protected set; }
    }
}
