namespace Store4Dev.Application.Commands
{
    public class CreateProductCommand
    {
        public string Name { get; set; }
     
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }

        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }

        public decimal CurrentStock { get; set; }
        public decimal MinStock { get; set; }
    }
}
