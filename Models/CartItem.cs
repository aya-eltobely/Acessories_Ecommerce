namespace AccessoriesWebsite.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Subtotal { get; set; }

        public virtual List<ProductCartItem> productCartItems { get; set; }


        public int? cartId { get; set; }
        public virtual Cart cart { get; set; }


    }
}
