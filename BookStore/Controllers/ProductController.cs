using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Controllers
{
    public class ProductController : Controller
    {
        public readonly IProductService productService;
        public readonly ICategoryService categoryService;

        public ProductController(IProductService productService = null, ICategoryService categoryService = null)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }
        [HttpPost]
        public async Task<IActionResult> Index2()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var data = await productService.GetProductsAsync(searchValue, skip, pageSize);
            var recordsTotal = data.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };
            return Ok(jsonData);
        }
        public async Task<IActionResult> Index()
        {
            //var products = await productService.GetProductsAsync();
            return View();
        }
        public async Task<IActionResult> Create()
        {
            var product = new CreateProduct();
            product.Categories = await categoryService.GetSelectListAsync();
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProduct product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            int num = await productService.CreateProduct(product);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            var product = await productService.GetProductUpdate(id);
            product.Categories = await categoryService.GetSelectListAsync();
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProduct updateProduct, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(updateProduct);
            }
            var product = await productService.ProductUpdate(updateProduct, id);
            if (!product.IsNullOrEmpty())
            {
                // Add a custom model state error
                ModelState.AddModelError("", product);

                // Return the view with the updated product model
                return View(product);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await productService.Delete(id);

            if (isDeleted)
            {
                // Optionally, you can return a JSON response instead of a redirect
                return Json(new { success = true, message = "product deleted successfully." });
                // Or, you can redirect to another action
                // return RedirectToAction(nameof(Index));
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                // Optionally, you can return a JSON response instead of BadRequest
                return BadRequest(new { error = "Failed to delete the product." });
            }
        }

        public async Task<ActionResult> CartView(int id)
        {
            var product = await productService.ProductCart(id);
            return View(product);

        }


    }
}
