namespace Store4Dev.Application.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        public Guid BrandId { get; set; }
        public string BrandName { get; set; }

        public string Name { get; set; }

        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }

        public decimal CurrentStock { get; set; }
        public decimal MinStock { get; set; }
    }
}
