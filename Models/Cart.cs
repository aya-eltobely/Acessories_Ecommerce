namespace AccessoriesWebsite.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public string? userId { get; set; }
        public virtual ApplicationUser user { get; set; }

        public virtual List<CartItem> cartItems { get; set; }

    }
}
