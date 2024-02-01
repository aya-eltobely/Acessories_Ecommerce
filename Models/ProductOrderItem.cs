namespace AccessoriesWebsite.Models
{
    public class ProductOrderItem
    {
        public int? productId { get; set; }
        public virtual Product product { get; set; }

        public int? orderItemId { get; set; }
        public virtual OrderItem orderItem { get; set; }
    }
}
