using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.Caategory;
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
        public async Task<IActionResult> Index()
        {

            return View();

        }

        public async Task<IActionResult> Indexjson()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var data = await _categoryService.ViewCategory(searchValue, skip, pageSize);
            var recordsTotal = data.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };
            return Ok(jsonData);

        }


        public async Task<IActionResult> CreateAsync()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CategoryCreateDataView categoryCreateDataView)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryCreateDataView);
            }
            var categories = await _categoryService.CreateCategory(categoryCreateDataView);
            return RedirectToAction(nameof(Index));
        }

    }
}
