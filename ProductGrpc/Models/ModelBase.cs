using ProductGrpc.Models.Includes;

namespace ProductGrpc.Models
{
    public class ModelBase<TKey>: IModel
    {
        public TKey Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
