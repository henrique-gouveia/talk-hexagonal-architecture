namespace Store4Dev.Domain.Support
{
    public interface IEntityRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        public IUnitOfWork UnitOfWork { get; }
    }
}
