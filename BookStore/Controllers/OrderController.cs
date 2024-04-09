using BookStore.BusnessLogic.Helper;
using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.Order;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Stripe.Checkout;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        private readonly IToastNotification toastNotification;

        public OrderController(IShoppingCartService shoppingCartService, IOrderService orderService, IToastNotification toastNotification)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
            this.toastNotification = toastNotification;
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

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(string status)
        {
            if (User.IsInRole("User"))
            {
                // Get the current user's identity
                ClaimsPrincipal currentUser = this.User;
                // Get the user's unique identifier (if needed)
                string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var data = await _orderService.GetUserOrders(status, userId);
                var recordsTotal = data.Count();
                var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };
                return Ok(jsonData);

            }
            else
            {
                var data = await _orderService.GetOrders(status);
                var recordsTotal = data.Count();
                var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };
                return Ok(jsonData);
            }



        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DetailsAsync(UpdateOrderView model)
        {
            var result = await _orderService.UpdateOrder(model.Header_Id, model);
            return RedirectToAction("Details", model.Header_Id);

        }
        [Authorize]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var order = await _orderService.GetDetails(id);
            return View(order);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> StartProcessing(int id)
        {
            var result = await _orderService.UpdateOrderStartProcessing(id);
            //return RedirectToAction("Details", "Order",id);
            toastNotification.AddErrorToastMessage("order is process");
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ShipOrder(UpdateOrderView model)
        {
            var result = await _orderService.UpdateOrderShipOrder(model);
            // return RedirectToAction("Details", model.Header_Id);
            toastNotification.AddErrorToastMessage("order is shipped");
            return RedirectToAction("Index");
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
        {
            await _orderService.UpdateOrderCancelOrder(id);
            // return RedirectToAction("Details", model.Header_Id);
            toastNotification.AddErrorToastMessage("order is canceled");
            return RedirectToAction("Index");
        }






    }
}
