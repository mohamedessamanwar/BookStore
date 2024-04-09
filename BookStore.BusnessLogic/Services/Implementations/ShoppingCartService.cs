using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.ShoppingCart;
using BookStore.DataAccessLayer.Interfaces;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace BookStore.BusnessLogic.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;


        public ShoppingCartService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<string> AddShoppingCart(AddToShoppingCart addToShoppingCart)
        {
            ShopingCart shopingCart = _unitOfWork.GetRepository<ShopingCart>().Find(s => s.UserId == addToShoppingCart.UserId && s.ProductId == addToShoppingCart.ProductId);
            if (shopingCart == null)
            {
                //create shoping cart 
                ShopingCart shopingCart1 = new ShopingCart
                {
                    UserId = addToShoppingCart.UserId,
                    Count = addToShoppingCart.Count,
                    ProductId = addToShoppingCart.ProductId
                };
                // save in db 
                await _unitOfWork.GetRepository<ShopingCart>().AddAsync(shopingCart1);
                int result = _unitOfWork.Complete();
                // return string 
                if (result == 0)
                {
                    return "Some Thing Wrong During Add To Cart";
                }
                return "";
            }
            else
            {
                // update count
                shopingCart.Count = addToShoppingCart.Count;
                // save change
                int result = _unitOfWork.Complete();
                // return string 
                if (result == 0)
                {
                    return "Some Thing Wrong During Add To Cart";
                }
                return "";


            }

        }

        public ShoppingCartDisplay GetShoppingCart(string id)
        {
            List<ShopingCart> shopingCart = _unitOfWork.GetRepository<ShopingCart>().FindAll(s => s.UserId == id, new[] { "Product" }).ToList();
            if (shopingCart == null)
            {
                return null;
            }
            ShoppingCartDisplay shoppingCartDisplays = new ShoppingCartDisplay();

            foreach (var ca in shopingCart)
            {
                shoppingCartDisplays.CartViews.Add(new CartView
                {
                    CartId = ca.Id,
                    Name = ca.Product.Name,
                    LastPrice = ca.Product.LastPrice,
                    imageUrl = ca.Product.ImageUrl,
                    count = ca.Count,
                    Author = ca.Product.Author,
                    Id = ca.Product.Id
                });
                shoppingCartDisplays.TotalPrice = shoppingCartDisplays.TotalPrice + (ca.Product.LastPrice * ca.Count);
            }

            return shoppingCartDisplays;

        }

        public async Task Plus(int cartId)
        {
            var shopingCart = await _unitOfWork.GetRepository<ShopingCart>().GetByIdAsync(cartId);
            shopingCart.Count += 1;
            _unitOfWork.Complete();
        }

        public async Task Minus(int cartId)
        {
            var shopingCart = await _unitOfWork.GetRepository<ShopingCart>().GetByIdAsync(cartId);
            shopingCart.Count -= 1;
            _unitOfWork.Complete();
        }

        public async Task ClearCart(string userid)
        {
            List<ShopingCart> shopingCart = _unitOfWork.GetRepository<ShopingCart>().FindAll(s => s.UserId == userid, new[] { "Product" }).ToList();
            //clear cart
            _unitOfWork.GetRepository<ShopingCart>().DeleteRange(shopingCart);
            _unitOfWork.Complete();
        }
    }
}
