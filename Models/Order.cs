namespace AccessoriesWebsite.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string? userId { get; set; }

        public int TotalAmount { get; set; }

        public virtual ApplicationUser user { get; set; }

        public virtual List<OrderItem> orderItems { get; set; }
    }
}
