using ProductGrpc.Models.Enums;

namespace ProductGrpc.Models
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public float Price { get; private set; }
        public ProductStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }


        public Product(Guid id, string name, string description, float price, ProductStatus status, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Status = status;
            CreatedAt = createdAt;
        }

        public Product(string name, string description, float price, ProductStatus status)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Status = status;
            CreatedAt = DateTime.Now;
        }

    }

}
