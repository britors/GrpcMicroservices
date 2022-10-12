using ProductGrpc.Models.Enums;

namespace ProductGrpc.Models
{
    public class Product: ModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public ProductStatus Status { get; set; }
        
    }

}
