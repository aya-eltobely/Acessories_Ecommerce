using AccessoriesWebsite.Models;

namespace AccessoriesWebsite.ViewModels
{
    public class OrderWithOrderItems
    {
        public int orderId { get; set; }
        public string userName { get; set; }
        public int totalAmount { get; set; }
        public List<ProductDetailViewModel> productDetailViewModels { get; set; }
    }
}
