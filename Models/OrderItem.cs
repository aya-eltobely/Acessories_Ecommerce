namespace AccessoriesWebsite.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Subtotal { get; set; }

        public int? orderId { get; set; }
        public virtual Order order { get; set; }

        public virtual List<ProductOrderItem> productOrderItems { get; set; }


    }
}
