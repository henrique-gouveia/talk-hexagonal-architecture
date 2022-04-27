namespace Store4Dev.Domain.Support
{
    public interface IUnitOfWork
    {
        public Task<bool> CompleteAsync();
    }
}
