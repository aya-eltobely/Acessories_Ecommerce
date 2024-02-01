using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using Microsoft.AspNetCore.Authorization;
using AccessoriesWebsite.Models;
using AccessoriesWebsite.ViewModels;
using System.Security.Claims;
using AccessoriesWebsite.Interfaces;
using Product = AccessoriesWebsite.Models.Product;
using Stripe.Issuing;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Stripe.Climate;
using Order = AccessoriesWebsite.Models.Order;


namespace AccessoriesWebsite.Controllers
{

    public class PaymentsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public PaymentsController(IUnitOfWork _unitOfWork , UserManager<ApplicationUser> _userManager )
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
            //StripeConfiguration.ApiKey = "sk_test_51OUBXdD2Hwtq7d2Z0hei9SM5AewU8B5eLcmMAixE0ipYdqAC7VXjeE4zXt89s5vFC3OQCK9dy8TqFMjCrXy20T2J00LsBmgIPs";
        }

        //[Authorize]
        //[HttpPost(Name = "CreateCheckoutSession")]
        //public IActionResult CreateCheckoutSession()
        //{
        //    var options = new SessionCreateOptions
        //    {
        //        LineItems = new List<SessionLineItemOptions>
        //    {
        //      new SessionLineItemOptions
        //      {
        //        PriceData = new SessionLineItemPriceDataOptions
        //        {
        //          UnitAmount = 2000,
        //          Currency = "usd",
        //          ProductData = new SessionLineItemPriceDataProductDataOptions
        //          {
        //            Name = "T-shirt",
        //          },
        //        },
        //        Quantity = 1,
        //      },
        //    },
        //        Mode = "payment",
        //        SuccessUrl = Url.Action("Success", "Payments", null, Request.Scheme),
        //        CancelUrl = "http://localhost:4242/cancel",
        //    };

        //    var service = new SessionService();
        //    Session session = service.Create(options);

        //    Response.Headers.Add("Location", session.Url);
        //    return new StatusCodeResult(303);
        //}


        [Authorize]

        [HttpPost]
        public IActionResult CheckPayment([FromBody] CartWithTotalVM cartDataWithTotal)
        {
            string cartDataJson = JsonSerializer.Serialize(cartDataWithTotal.cartData);
            TempData["CartData"] = cartDataJson;
            TempData["totalProduct"] = cartDataWithTotal.allTotalProduct;
            return Json(new { success = true, redirectUrl = Url.Action("ShowPayment", "Payments") });     
        }

        [Authorize]

        public async Task< IActionResult >ShowPayment(UserDataOrderVM userData)
        {
            string data = (string)TempData.Peek("CartData");
            var totalProduct = TempData.Peek("totalProduct");
            ViewData["TotalProduct"] = totalProduct;


            List<CartVM> deserializedCartData = JsonSerializer.Deserialize<List<CartVM>>(data);
            ViewData["CartData"] = deserializedCartData;

            if(userData != null)
            {
                if (ModelState.IsValid)
                {
                    //save user data
                    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        user.FirstName = userData.FirstName;
                        user.LastName = userData.LastName;
                        //user.Email = userData.Email;
                        user.Address = userData.Address;
                        user.Country = userData.Country;
                        user.State = userData.State;
                        user.Zip = userData.Zip;
                        unitOfWork.AppUser.Update(user);
                    }

                    //// save order in database

                    Order order = new Order()
                    {
                        userId = userId,
                        TotalAmount = (int)totalProduct
                    };
                    unitOfWork.Order.Create(order);

                    //////////////////////////////////////////////////////////
                    // ProductOrderitem
                    List<ProductOrderItem> productOrderItems = new List<ProductOrderItem>();

                    // Create order items and product order items simultaneously
                    foreach (var item in deserializedCartData)
                    {
                        OrderItem orderItem = new OrderItem
                        {
                            Quantity = item.Quantity,
                            Subtotal = item.Subtotal,
                            orderId = order.Id,
                        };

                        // Create and add order item to the database
                        unitOfWork.OrderItem.Create(orderItem);

                        // Create product order item
                        ProductOrderItem productOrderItem = new ProductOrderItem
                        {
                            productId = item.ProdId,
                            orderItemId = orderItem.Id, // Set the order item Id here
                        };

                        // Add product order item to the list
                        productOrderItems.Add(productOrderItem);
                    }

                    // Create and save product order items in bulk
                    unitOfWork.ProductOrderItem.CreateRange(productOrderItems);

                    ///////////////////////////////////////////////////////////////////

                    ////orderitem
                    //List<OrderItem> orderItems = new List<OrderItem>();
                    //orderItems.AddRange(deserializedCartData.Select(item => new OrderItem
                    //{
                    //    Quantity = item.Quantity,
                    //    Subtotal = item.Subtotal,
                    //    orderId = order.Id,      
                    //}));
                    //unitOfWork.OrderItem.CreateRange(orderItems);


                    ///
                    // Update product stock
                    foreach (var item in deserializedCartData)
                    {
                        Product product = unitOfWork.Product.GetById(item.ProdId);
                        if (product != null)
                        {
                            product.InStock -= item.Quantity;
                            unitOfWork.Save();
                        }
                    }

                    /// empty local storage



                    return RedirectToAction("Success", "Payments");
                }
                else
                {
                    return View(userData);
                }
            }

            return View();
        }


        [Authorize]

        public IActionResult Success()
        {
            return View();
        }

        
        public IActionResult GetAuthenticationStatus()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            return Json(new { isAuthenticated = isAuthenticated });
        }

    }
}
