using AccessoriesWebsite.Helpers;
using AccessoriesWebsite.Implementation;
using AccessoriesWebsite.Interfaces;
using AccessoriesWebsite.Models;
using AccessoriesWebsite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Climate;
using Product = AccessoriesWebsite.Models.Product;

namespace AccessoriesWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        //get all products    
        public IActionResult Index(int? page, string? search)
        {

            int pageSize = 5; // Set your desired page size
            int pageNumber = page ?? 1;

            List<Product> products = unitOfWork.Product.GetAll("category").ToList();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(d => d.Name.ToLower().Contains(search.ToLower())).ToList();
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
            };

            return View(allProducts);
        }


        // GET: Product/Create
        public IActionResult Create()
        {
            List<Category> categories = unitOfWork.Category.GetAll().ToList();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            return View();
        }


        // POST: Product/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductVM product, IFormFile image)
        {
            List<Category> categories = unitOfWork.Category.GetAll().ToList();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                    var filePath = Path.Combine(uploads, image.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    Product newProduct = new Product();
                    newProduct.Name = product.Name;
                    newProduct.Description = product.Description;
                    newProduct.Price = product.Price;
                    newProduct.InStock = product.InStock;
                    newProduct.categoryId = product.categoryId;
                    newProduct.ImageUrl = image.FileName;


                    unitOfWork.Product.Create(newProduct);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Image", "Please upload an image.");
                }
            }

            return View(product);
        }


        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = unitOfWork.Product.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            ProductVM productVM = new ProductVM();
            productVM.Name = product.Name;
            productVM.Description = product.Description;
            productVM.Price = product.Price;
            productVM.ImageUrl = product.ImageUrl;
            productVM.InStock = product.InStock;
            productVM.categoryId = product.categoryId;


            List<Category> categories = unitOfWork.Category.GetAll().ToList();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.categoryId);

            return View(productVM);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductVM product, IFormFile? image)
        {    
            if (ModelState.IsValid)
            {
                Product oldProduct = unitOfWork.Product.GetById(id);

                if (image != null && image.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                    var filePath = Path.Combine(uploads, image.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    oldProduct.ImageUrl = image.FileName;
                }
                //else
                //{
                //    return NotFound();
                //}

                oldProduct.Name = product.Name;
                oldProduct.Description = product.Description;
                oldProduct.Price = product.Price;
                oldProduct.InStock = product.InStock;
                oldProduct.categoryId = product.categoryId;

                unitOfWork.Product.Update(oldProduct);

                return RedirectToAction(nameof(Index));
            }
            List<Category> categories = unitOfWork.Category.GetAll().ToList();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.categoryId);
            return View(product);
        }

        // Details: Product/Details/5
        public IActionResult Details(int id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Product product = unitOfWork.Product.GetById(id , "category");

            if (id == null)
            {
                return NotFound();
            }

            return View(product);
        }



        // Delete: Product/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = unitOfWork.Product.GetById(id, "category");

            if (id == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartialView",product);
            //return View(product);
        }

        [HttpPost]
        public IActionResult DeleteProd(int Id)
        {
            Product product = unitOfWork.Product.GetById(Id, "");

            if (Id == null)
            {
                return NotFound();
            }

            var res = unitOfWork.Product.Delete(product); 

            //true ==> deledted done
            if(res)
            {
                return RedirectToAction("Index");
            }
            
            return NotFound("wrong");
            
        }


        public IActionResult callProductsUsingAjax()
        {
            return Json(new { success = true, redirectUrl = Url.Action("Index", "Product") });

        }
    }
}
