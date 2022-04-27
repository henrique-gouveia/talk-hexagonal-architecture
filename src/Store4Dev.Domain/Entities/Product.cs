using Store4Dev.Domain.Support;

namespace Store4Dev.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        protected Product() { }

        public Product(
            Brand brand,
            string name,
            decimal costPrice,
            decimal salePrice,
            decimal currentStock,
            decimal minStock = 0,
            bool active = true)
        {
            Assertion.NotNull(brand, "Brand must not be null");
            Assertion.NotNullOrEmpty(name, "Name must not be null or empty");
            Assertion.GreaterThanEqual(costPrice, 0, "Cost Price must not be negative");
            Assertion.GreaterThanEqual(salePrice, 0, "Sales Price must not be negative");
            Assertion.GreaterThanEqual(salePrice, costPrice, "Sales Price must be greater than Cost Price");

            Brand = brand;
            BrandId = brand.Id;
            Name = name;
            CostPrice = costPrice;
            SalePrice = salePrice;
            CurrentStock = currentStock;
            MinStock = minStock;
            Active = active;
        }

        public Guid BrandId { get; private set; }
        public Brand Brand { get; private set; }

        public string Name { get; private set; }

        public decimal CostPrice { get; private set; }
        public decimal SalePrice { get; private set; }

        public decimal CurrentStock { get; private set; }
        public decimal MinStock { get; private set; }

        public bool Active { get; private set; }

        public decimal Profit()
            => ((SalePrice / CostPrice) - 1) * 100;

        public void ChangeProfit(decimal profit)
        {
            Assertion.GreaterThanEqual(profit, 0, "Profit must not be negative");
            SalePrice = CostPrice + (CostPrice * (profit / 100));
        }

        public void IcreaseStock(decimal value)
        {
            Assertion.GreaterThanEqual(value, 0, "Value must not be negative");
            CurrentStock += value;
        }

        public void DecreaseStock(decimal value)
        {
            decimal stock = Math.Abs(value);
            CurrentStock -= stock;
        }

        public void Enable() => Active = true;
        public void Disable() => Active = false;
    }
}