namespace ProductGrpc.Models
{
    public class ProductStatus : ModelBase<int>
    {
        public string Name { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
