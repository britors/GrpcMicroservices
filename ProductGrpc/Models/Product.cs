using ProductGrpc.Models.Enums;

namespace ProductGrpc.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public ProductStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
