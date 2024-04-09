
using BookStore.BusnessLogic.Helper;
using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.Product;
using BookStore.BusnessLogic.ViewsModels.ShoppingCart;
using BookStore.DataAccessLayer.Interfaces;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
namespace BookStore.BusnessLogic.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ProductView>> GetProductsAsync(string searchValue, int skip, int take)
        {
            List<Product> products = (await _unitOfWork.GetRepository<Product>().Fillter(searchValue.IsNullOrEmpty() ? null : m =>
               m.Name.Contains(searchValue) || m.Author.Contains(searchValue), skip, take, new[] { "Category" })).ToList<Product>();
            return products.Select(p => new ProductView
            {
                Id = p.Id,
                Price = p.Price,
                Author = p.Author,
                Name = p.Name,
                Category = p.Category.Name


            }).ToList();
        }

        public async Task<int> CreateProduct(CreateProduct createProduct)
        {
            //save image in Local ...
            string image = await SaveImage(createProduct.Image);
            //mapping ...

            var product = _unitOfWork.GetRepository<Product>().AddAsync(new Product
            {
                ImageUrl = image,
                Name = createProduct.Name,
                Description = createProduct.Description,
                Price = createProduct.Price,
                Author = createProduct.Author,
                LastPrice = createProduct.LastPrice,
                active = createProduct.active,
                CategoryId = createProduct.CategoryId
            });
            return _unitOfWork.Complete();
        }

        public async Task<String> SaveImage(IFormFile image)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var path = Path.Combine(Seting.ImagesPath, coverName);
            using var stream = File.Create(path);
            await image.CopyToAsync(stream);
            return coverName;
        }

        public async Task<UpdateProduct> GetProductUpdate(int id)
        {
            Product product = _unitOfWork.GetRepository<Product>().Find(p => p.Id == id, new[] { "Category" });

            return new UpdateProduct
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Author = product.Author,
                LastPrice = product.LastPrice,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                active = product.active
            };


        }
        public async Task<string> ProductUpdate(UpdateProduct updateProduct, int id)
        {
            Product product = _unitOfWork.GetRepository<Product>().Find(p => p.Id == id, new[] { "Category" });
            if (product == null)
            {
                return "Product is not exixt";
            }
            string old = product.ImageUrl;
            var image = updateProduct.Image;
            string currunt = "";


            product.Price = updateProduct.Price;
            product.LastPrice = updateProduct.LastPrice;
            //product.ImageUrl = currunt;
            product.CategoryId = updateProduct.CategoryId;
            product.Description = updateProduct.Description;
            product.Author = updateProduct.Author;
            product.Name = updateProduct.Name;
            product.active = updateProduct.active;

            if (image != null)
            {
                currunt = await SaveImage(image);
                product.ImageUrl = currunt;
            }
            _unitOfWork.GetRepository<Product>().Update(product); ;
            var result = _unitOfWork.Complete();
            if (result > 0 && image != null)
            {
                var coverr = Path.Combine(Seting.ImagesPath, old);
                File.Delete(coverr);
                return "";
            }
            if (result < 0 && image != null)
            {
                var cover = Path.Combine(Seting.ImagesPath, product.ImageUrl);
                File.Delete(cover);
                return "product is not update";
            }



            return "";
        }

        public async Task<bool> Delete(int id)
        {
            Product product = _unitOfWork.GetRepository<Product>().GetById(id);

            if (product == null)
            {
                return false;
            }

            _unitOfWork.GetRepository<Product>().Delete(product);
            int result = _unitOfWork.Complete();
            if (result > 0)
            {
                return true;
            }
            return false;


        }

        public async Task<List<ProductView>> GetProductsAsyncWithoutFillter(string name)
        {
            List<Product> products = (await _unitOfWork.GetRepository<Product>().FindWithIncludsAsync(new[] { "Category" })).ToList();
            if (name != null)
            {
                products = products.Where(p => p.Name.Contains(name)).ToList();

            }
            return products.Select(p => new ProductView
            {
                Id = p.Id,
                Price = p.Price,
                Author = p.Author,
                Name = p.Name,
                Category = p.Category.Name,
                imageUrl = p.ImageUrl,
                LastPrice = p.LastPrice


            }).ToList();
        }

        public async Task<CartView> ProductCart(int id)
        {
            var p = (_unitOfWork.GetRepository<Product>().Find(p => p.Id == id, new[] { "Category" }));
            return new CartView
            {
                Id = p.Id,
                Price = p.Price,
                Author = p.Author,
                Name = p.Name,
                Category = p.Category.Name,
                imageUrl = p.ImageUrl,
                LastPrice = p.LastPrice


            };
        }
    }
}
