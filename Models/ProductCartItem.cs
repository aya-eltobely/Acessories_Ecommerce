namespace AccessoriesWebsite.Models
{
    public class ProductCartItem
    {
        public int? productId { get; set; }
        public virtual Product product { get; set; }

        public int? cartItemId { get; set; }
        public virtual CartItem cartItem { get; set; }
    }
}
