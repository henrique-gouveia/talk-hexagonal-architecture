namespace Store4Dev.Domain.Services
{
    public interface IStockService
    {
        Task<bool> DecreaseStockAsync(Guid productId, decimal quantity);
        Task<bool> IncreaseStockAsync(Guid productId, decimal quantity);
    }
}
