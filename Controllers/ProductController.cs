using AccessoriesWebsite.Helpers;
using AccessoriesWebsite.Interfaces;
using AccessoriesWebsite.Models;
using AccessoriesWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace AccessoriesWebsite.Controllers
{
    public class ProductController : Controller
    {
        private IUnitOfWork unitOfWork { get; }

        public ProductController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }


        public IActionResult Index(int? page, string? search,int? catId)
        {
            int pageSize = 5; // Set your desired page size
            int pageNumber = page ?? 1;

            List<Product> products =  unitOfWork.Product.GetAll().ToList();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(d => d.Name.ToLower().Contains(search.ToLower())).ToList(); 
            }

            // Apply search filter
            if (catId.HasValue)
            {
                products = products.Where(d => d.categoryId == catId).ToList();
            }

            int totalCount = products.Count;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var pagedData = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            //var pagedData = products.ToPagedList(pageNumber, pageSize);

            var allProducts = new ProductPageResult<Product>
            {
                Data = pagedData.ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItem = totalCount,
                TotalPages = totalPages,
                search = search,
                catId = catId
            };

            List<Category> categories =  unitOfWork.Category.GetAll().ToList();
            ViewBag.category = categories;

            return View(allProducts);
        }

        public IActionResult ProductDetails(int prodId)
        {
            Product prod = unitOfWork.Product.GetById(prodId);
            return View(prod);

        }


        public IActionResult callProductsUsingAjax()
        {
            return Json(new { success = true, redirectUrl = Url.Action("Index", "Product") });

        }
    }
}
