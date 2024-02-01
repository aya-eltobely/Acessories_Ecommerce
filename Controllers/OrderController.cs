using AccessoriesWebsite.Implementation;
using AccessoriesWebsite.Interfaces;
using AccessoriesWebsite.Models;
using AccessoriesWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Security.Claims;

namespace AccessoriesWebsite.Controllers
{

    //[Authorize(Roles = "User")]
    public class OrderController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private readonly IUnitOfWork unitOfWork;

        public ApplicationDBContext context { get; }

        public OrderController(UserManager<ApplicationUser> _userManger, SignInManager<ApplicationUser> _signInManager,IUnitOfWork _unitOfWork,
            ApplicationDBContext _context)
        {
            userManager = _userManger;
            signInManager = _signInManager;
            unitOfWork = _unitOfWork;
            context = _context;
        }

        public async Task<IActionResult> Index()
        {

            //user Name
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }


            // Fetch order details using Entity Framework or your preferred data access mechanism

            List<OrderWithOrderItems> orders = context.orders
           .Where(o => o.userId == userId)
           .Select(o => new OrderWithOrderItems
           {
               orderId = o.Id,
               totalAmount = o.TotalAmount,
               userName = user.UserName,
               productDetailViewModels = o.orderItems
                   .Select(oi => new ProductDetailViewModel
                   {
                       //error here////////////////////////////////////////////////////////
                       //imageName = oi.productOrderItems.Select(p => p.product.ImageUrl).FirstOrDefault().ToString(),
                       ////oi.productOrderItems.Select(p => p.product.ImageUrl)?.FirstOrDefault()?.ToString(),
                       //prodName = oi.productOrderItems.Select(p => p.product.Name).FirstOrDefault().ToString(),
                       ////oi.productOrderItems.Select(p => p.product.Name)?.FirstOrDefault()?.ToString(),
                       quantity = oi.Quantity,
                       subtotal = oi.Subtotal,
                       prodName = oi.productOrderItems.Select(poi => poi.product.Name).FirstOrDefault(),
                       imageName = oi.productOrderItems.Select(p => p.product.ImageUrl).FirstOrDefault(),


                   })
                   .ToList()
           })
           .OrderByDescending(o => o.orderId)
           .ToList();

            return View(orders);

        }
    }
}


//public async Task<IActionResult> Index()
//{

//    //user Name
//    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//    var user = await userManager.FindByIdAsync(userId);

//    if (user == null)
//    {
//        return NotFound();
//    }

//    List<OrderWithOrderItems> orderWithOrderItems = new List<OrderWithOrderItems>();

//    List<Order> orders = unitOfWork.Order.GetAll(o => o.userId == userId, null, "").ToList();

//    //get all order items

//    //foreach (var order in orders)
//    //{
//    //    OrderWithOrderItems orderWithOrderItem = new OrderWithOrderItems();


//    //    orderWithOrderItem.orderId = order.Id;
//    //    orderWithOrderItem.userName = user.FirstName + " " + user.LastName;
//    //    orderWithOrderItem.totalAmount = order.TotalAmount;


//    //    List<ProductOrderItems> productOrderItems = new List<ProductOrderItems>();
//    //    //all order items
//    //    List<OrderItem> orderItems = unitOfWork.OrderItem.GetAll(oi => oi.orderId == order.Id, null, "").ToList();

//    //    //List<ProductOrderItem> productsorderItems = unitOfWork.ProductOrderItem.GetAll(oi => oi.orderItemId == order.Id, null, "").ToList();

//    //    //List<ProductOrderItems> productOrderitems = new List<ProductOrderItems>();
//    //    //productOrderItems.AddRange(orderItems.Select(item => new ProductOrderItems
//    //    //{
//    //    //    prodName =
//    //    //}));

//    //    orderWithOrderItem.ProductOrderItems = productOrderItems;


//    //    orderWithOrderItems.Add(orderWithOrderItem);
//    //}

//    return View(orderWithOrderItems);
//}