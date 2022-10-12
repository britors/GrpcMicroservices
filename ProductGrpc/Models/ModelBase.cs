using ProductGrpc.Models.Includes;

namespace ProductGrpc.Models
{
    public class ModelBase: IModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
