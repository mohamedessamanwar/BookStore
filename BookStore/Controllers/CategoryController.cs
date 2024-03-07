using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.Caategory;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> IndexAsync()
        {
           var categories = await _categoryService.ViewCategory();
            return View(categories);
        }
      
        public async Task<IActionResult> CreateAsync()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CategoryCreateDataView categoryCreateDataView)
        {
            if(!ModelState.IsValid)
            {
                return View(categoryCreateDataView);
            }
            var categories = await _categoryService.CreateCategory(categoryCreateDataView);
            return RedirectToAction(nameof(IndexAsync));
        }

    }
}
