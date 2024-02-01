using AccessoriesWebsite.Interfaces;
using AccessoriesWebsite.Models;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;

namespace AccessoriesWebsite.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext Context;
        private readonly UserManager<ApplicationUser> userManager;

        public IBaseRepo<Cart> Cart { get; set; }
        public IBaseRepo<ApplicationUser> AppUser { get; set; }
        public IBaseRepo<CartItem> CartItem { get; set; }
        public IBaseRepo<Category> Category { get; set; }
        public IBaseRepo<Order> Order { get; set; }
        public IBaseRepo<OrderItem> OrderItem { get; set; }
        public IProductRepo Product { get; set; }
        public IBaseRepo<ProductCartItem> ProductCartItem { get; set; }
        public IBaseRepo<ProductOrderItem> ProductOrderItem { get; set; }



        public UnitOfWork(ApplicationDBContext context, UserManager<ApplicationUser> _userManager)
        {
            Context = context;
            userManager = _userManager;
            Cart = new BaseRepo<Cart>(Context);
            AppUser = new BaseRepo<ApplicationUser>(Context);
            CartItem = new BaseRepo<CartItem>(Context);
            Category = new BaseRepo<Category>(Context);
            Order = new BaseRepo<Order>(Context);
            OrderItem = new BaseRepo<OrderItem>(Context);
            Product = new ProductRepo(Context);
            ProductCartItem = new BaseRepo<ProductCartItem>(Context);
            ProductOrderItem = new BaseRepo<ProductOrderItem>(Context);
        }


        public void Dispose()
        {
            Context.Dispose();
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
