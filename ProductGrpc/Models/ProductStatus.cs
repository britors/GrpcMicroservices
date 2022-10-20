namespace ProductGrpc.Models
{
    public class ProductStatus : ModelBase<short>
    {
        public string Name { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
