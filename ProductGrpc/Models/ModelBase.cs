using ProductGrpc.Models.Includes;

namespace ProductGrpc.Models
{
    public class ModelBase: IModel
    {
        public Guid Id { get; set; }
    }
}
