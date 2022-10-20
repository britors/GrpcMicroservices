using ProductGrpc.Models.Enums;

namespace ProductGrpc.Models
{
    public class Product: ModelBase<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public short StatusId {get;set; }
        public virtual ProductStatus Status { get; set; }
    }

}
