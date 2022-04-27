using Store4Dev.Domain.Support;

namespace Store4Dev.Domain.Entities
{
    public class Brand : Entity
    {
        protected Brand() { }

        public Brand(string name)
        {
            Assertion.NotNullOrEmpty(name, "Name must not be null or empty");

            Name = name;
            Products = new List<Product>();
        }

        public static Brand New(Guid id, string name)
            => new (name) { Id = id };

        public string Name { get; private set; }

        public ICollection<Product> Products { get; private set; }
    }
}
