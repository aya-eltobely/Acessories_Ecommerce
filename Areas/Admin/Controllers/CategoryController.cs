using AccessoriesWebsite.Areas.Admin.ViewModels;
using AccessoriesWebsite.Interfaces;
using AccessoriesWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccessoriesWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        //get all category
        public IActionResult Index()
        {
            List<Category> categories = unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }


        //create category
        public IActionResult Create()
        {
            return View();
            //AddCatVM category = new AddCatVM();
            //return PartialView("_CreaetPartialView", category);
        }

        //create category
        [HttpPost]
        public IActionResult Create(AddCatVM category)
        {
            if (ModelState.IsValid)
            {
                Category newCategory = new Category();
                newCategory.Name = category.Name;
                Category c = unitOfWork.Category.Create(newCategory);
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
                //return PartialView("_CreaetPartialView", category);

                //return RedirectToAction("Index");
            }
        }

        
        //Edit category
        public IActionResult Edit(int id)
        {
            Category category = unitOfWork.Category.GetById(id);

            //return PartialView( "_EditPartialView", category);
            return View(category);
        }

        //Edit category
        [HttpPost]
        public IActionResult Edit(int Id,AddCatVM newCategory)
        {
            if (ModelState.IsValid)
            {
                Category oldCategory = unitOfWork.Category.GetById(Id);
                oldCategory.Name = newCategory.Name;
                unitOfWork.Category.Update(oldCategory);
                return RedirectToAction("Index");
            }

            return View(newCategory);
            

            
        }


        //Details category
        public IActionResult Details(int id)
        {
            Category category = unitOfWork.Category.GetById(id);
            return View(category);
        }

        //Delete category
        public IActionResult Delete(int id)
        {
            Category category = unitOfWork.Category.GetById(id);
            return PartialView("_DeletePartialView", category);

            //return View(category);
        }

        //Delete category
        [HttpPost]
        public IActionResult Delete(Category c)
        {
            Category category = unitOfWork.Category.GetById(c.Id);
            unitOfWork.Category.Delete(category);
            return RedirectToAction("Index");
        }
    }
}
