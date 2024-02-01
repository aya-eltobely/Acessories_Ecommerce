using AccessoriesWebsite.Models;
using static System.Net.Mime.MediaTypeNames;

namespace AccessoriesWebsite.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepo<Cart> Cart { get; }
        IBaseRepo<ApplicationUser> AppUser { get; }
        IBaseRepo<CartItem> CartItem { get; }
        IBaseRepo<Category> Category { get; }
        IBaseRepo<Order> Order { get; }
        IBaseRepo<OrderItem> OrderItem { get; set; }
        IProductRepo Product { get; set; }
        IBaseRepo<ProductCartItem> ProductCartItem { get; set; }
        IBaseRepo<ProductOrderItem> ProductOrderItem { get; set; }

        void Save();
    }
}
