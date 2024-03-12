using BookStore.BusnessLogic.ViewsModels.Product;
using BookStore.BusnessLogic.ViewsModels.ShoppingCart;

namespace BookStore.BusnessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<int> CreateProduct(CreateProduct createProduct);
        Task<bool> Delete(int id);
        Task<List<ProductView>> GetProductsAsync(string searchValue, int skip, int take);
        Task<UpdateProduct> GetProductUpdate(int id);
        Task<string> ProductUpdate(UpdateProduct updateProduct, int id);
        Task<List<ProductView>> GetProductsAsyncWithoutFillter();

        Task<CartView> ProductCart(int id);
    }
}
