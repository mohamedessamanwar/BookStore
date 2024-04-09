using BookStore.BusnessLogic.Helper;
using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.Order;
using BookStore.BusnessLogic.ViewsModels.ShoppingCart;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Stripe.Checkout;
using System.Security.Claims;

public class ShoppingCartController : Controller
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IOrderService _orderService;

    public ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService = null)
    {
        _shoppingCartService = shoppingCartService;
        _orderService = orderService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToCart([FromBody] AddToShoppingCart addToShoppingCart)
    {
        // Get the current user's identity
        ClaimsPrincipal currentUser = this.User;
        // Get the user's unique identifier (if needed)
        string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        addToShoppingCart.UserId = userId;

        // Send data to service 
        string result = await _shoppingCartService.AddShoppingCart(addToShoppingCart);

        if (result.IsNullOrEmpty())
        {
            // Construct a success response
            var response = new
            {
                success = true,
                message = "add to cart"
            };
            return Json(response);
        }
        var response1 = new
        {
            success = false,
            message = result
        };

        // Return success JSON response
        return Json(response1);
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        // Get the current user's identity
        ClaimsPrincipal currentUser = this.User;
        // Get the user's unique identifier (if needed)
        string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // Send data to service 
        var result = _shoppingCartService.GetShoppingCart(userId);
        return View(result);

    }
    [Authorize]
    public async Task<IActionResult> Summary()
    {
        // Get the current user's identity
        ClaimsPrincipal currentUser = this.User;
        // Get the user's unique identifier (if needed)
        string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // Send data to service 
        var result = await _orderService.Summary(userId);
        return View(result);

    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Summary(OrderSummary orderSummary)
    {
        // Get the current user's identity
        ClaimsPrincipal currentUser = this.User;
        // Get the user's unique identifier (if needed)
        string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // Send data to service 
        var confirm = await _orderService.CreateOrder(orderSummary, userId);
        //stripe settings

        var domain = "https://localhost:44326/";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>(),

            Mode = "payment",
            SuccessUrl = domain + $"ShoppingCart/OrderConfirmation?id={confirm.orderId}",
            CancelUrl = domain + $"ShoppingCart/Index",
        };

        foreach (var item in confirm.shopingCarts)
        {

            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.Product.LastPrice * 100),
                    Currency = "try",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Name
                    },

                },
                Quantity = item.Count,
            };
            options.LineItems.Add(sessionLineItem);
        }

        var service = new SessionService();
        Session session = service.Create(options);
        await _orderService.UpdateStripePaymentId(confirm.orderId, session.Id, session.PaymentIntentId);
        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);


    }

    public async Task<IActionResult> OrderConfirmation(int id)
    {
        OrderHeader orderHeader = await _orderService.GetOrderAsync(id);
        var service = new SessionService();
        Session session = service.Get(orderHeader.SessionId);
        //check stripe status
        if (session.PaymentStatus.ToLower() == "paid")
        {
            await _orderService.UpdateStripePaymentId(id, orderHeader.SessionId, session.PaymentIntentId);
            await _orderService.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
        }
        // Get the current user's identity
        ClaimsPrincipal currentUser = this.User;
        // Get the user's unique identifier (if needed)
        string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _shoppingCartService.ClearCart(userId);
        return View();
    }

    public async Task<IActionResult> Plus(int cartId)
    {
        await _shoppingCartService.Plus(cartId);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Minus(int cartId)
    {
        await _shoppingCartService.Minus(cartId);
        return RedirectToAction("Index");
    }






}



