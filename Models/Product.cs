namespace AccessoriesWebsite.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int InStock { get; set; }

        public int? categoryId { get; set; }
        public virtual Category category { get; set; }

        public virtual List<ProductOrderItem> productOrderItems { get; set; }
        public virtual List<ProductCartItem> productCartItems { get; set; }

    }
}
