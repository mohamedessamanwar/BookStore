using BookStore.BusnessLogic.Helper;
using BookStore.BusnessLogic.Services.Interfaces;
using BookStore.BusnessLogic.ViewsModels.Order;
using BookStore.BusnessLogic.ViewsModels.ShoppingCart;
using BookStore.DataAccessLayer.Interfaces;
using BookStore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Stripe;

namespace BookStore.BusnessLogic.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;


        public OrderService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<OrderSummary> Summary(string userid)
        {
            List<ShopingCart> shopingCart = _unitOfWork.GetRepository<ShopingCart>().FindAll(s => s.UserId == userid, new[] { "Product" }).ToList();
            if (shopingCart == null)
            {
                return null;
            }

            OrderSummary orderSummary = new OrderSummary();

            foreach (var ca in shopingCart)
            {
                orderSummary.CartViews.Add(new CartView
                {
                    CartId = ca.Id,
                    Name = ca.Product.Name,
                    LastPrice = ca.Product.LastPrice,
                    imageUrl = ca.Product.ImageUrl,
                    count = ca.Count,
                    Author = ca.Product.Author,
                    Id = ca.Product.Id
                });
                orderSummary.OrderTotal = orderSummary.OrderTotal + (ca.Product.LastPrice * ca.Count);
            }

            ApplicationUser user = await _userManager.FindByIdAsync(userid);
            orderSummary.PhoneNumber = user.PhoneNumber;
            orderSummary.Name = $"{user.FirstName} + {user.LastName}";
            orderSummary.City = user.City;
            orderSummary.State = user.State;
            orderSummary.StreetAddress = user.Address;

            return orderSummary;
        }

        public async Task<OrderConfirmation> CreateOrder(OrderSummary orderSummary, string userid)
        {
            //Get Products 
            List<ShopingCart> shopingCart = _unitOfWork.GetRepository<ShopingCart>().FindAll(s => s.UserId == userid, new[] { "Product" }).ToList(); //////// Repo . 
                                                                                                                                                     // save user data 
            OrderHeader orderHeader = new OrderHeader();
            orderHeader.ApplicationUserId = userid;
            orderHeader.PhoneNumber = orderSummary.PhoneNumber;
            orderHeader.Name = orderSummary.Name;
            orderHeader.City = orderSummary.City;
            orderHeader.State = orderSummary.State;
            orderHeader.StreetAddress = orderSummary.StreetAddress;
            //save total order
            foreach (var ca in shopingCart)
            {
                orderHeader.OrderTotal = orderHeader.OrderTotal + (ca.Product.LastPrice * ca.Count);
            }
            //save status data 
            orderHeader.OrderStatus = SD.StatusPending;
            orderHeader.PaymentStatus = SD.PaymentStatusPending;
            orderHeader.OrderDate = DateTime.Now;
            // save order to data base
            _unitOfWork.GetRepository<OrderHeader>().Add(orderHeader);
            _unitOfWork.Complete();
            // save to Order detalis
            foreach (var ca in shopingCart)
            {
                _unitOfWork.GetRepository<OrderDetail>().Add(new OrderDetail
                {
                    OrderId = orderHeader.Id,
                    ProductId = ca.Product.Id,
                    Count = ca.Count,
                    Price = ca.Product.LastPrice
                });
                _unitOfWork.Complete();
            }

            return new OrderConfirmation
            {
                shopingCarts = shopingCart,
                orderId = orderHeader.Id,
            };


        }


        public async Task<OrderHeader> GetOrderAsync(int id)
        {
            return await _unitOfWork.GetRepository<OrderHeader>().GetByIdAsync(id);
        }


        public async Task UpdateStripePaymentId(int id, string SessionId, string PaymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<OrderHeader>().GetByIdAsync(id);
            order.SessionId = SessionId;
            order.PaymentIntentId = PaymentIntentId;
            _unitOfWork.Complete();
        }

        public async Task UpdateStatus(int id, string orderStatus, string paymentStatus)
        {
            var order = _unitOfWork._orderHeaderRepository.GetByIdAsync(id);
            //var order = await _unitOfWork.GetRepository<OrderHeader>().GetByIdAsync(id);
            order.PaymentStatus = paymentStatus;
            order.OrderStatus = orderStatus;
            _unitOfWork.Complete();

        }

        public async Task<List<OrderTableView>> GetOrders(string status)
        {
            var orderHeaders = _unitOfWork._orderHeaderRepository.GetOrderWithUser();
            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return orderHeaders.Select(o => new OrderTableView
            {
                Id = o.Id,
                Name = o.Name,
                PhoneNumber = o.PhoneNumber,
                OrderStatus = o.OrderStatus,
                OrderTotal = o.OrderTotal,
                Email = o.ApplicationUser.Email

            }).ToList();
        }
        public async Task<List<OrderTableView>> GetUserOrders(string status, string id)
        {
            var orderHeaders = _unitOfWork._orderHeaderRepository.GetUserOrderWithUser(id);
            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return orderHeaders.Select(o => new OrderTableView
            {
                Id = o.Id,
                Name = o.Name,
                PhoneNumber = o.PhoneNumber,
                OrderStatus = o.OrderStatus,
                OrderTotal = o.OrderTotal,
                Email = o.ApplicationUser.Email

            }).ToList();
        }

        public async Task<OrderView> GetDetails(int id)
        {
            var Details = _unitOfWork.orderDetailsReposatory.GetOrderDetails(id);
            var Header = _unitOfWork._orderHeaderRepository.GetOrder(id);
            return new OrderView
            {
                Detail = Details.ToList(),
                Header = Header
            };
        }
        public async Task<int> UpdateOrder(int id, UpdateOrderView updateOrderView)
        {
            var Header = _unitOfWork._orderHeaderRepository.GetOrder(id);
            Header.City = updateOrderView.Header_City;
            Header.StreetAddress = updateOrderView.Header_StreetAddress;
            Header.PhoneNumber = updateOrderView.Header_PhoneNumber;
            Header.Name = updateOrderView.Header_Name;
            Header.State = updateOrderView.Header_State;
            if (updateOrderView.Header_Carrier != null)
            {
                Header.Carrier = updateOrderView.Header_Carrier;
            }
            if (updateOrderView.Header_TrackingNumber != null)
            {
                Header.TrackingNumber = updateOrderView.Header_TrackingNumber;
            }
            return _unitOfWork.Complete();

        }

        public async Task<int> UpdateOrderStartProcessing(int id)
        {
            var Header = _unitOfWork._orderHeaderRepository.GetOrder(id);
            Header.OrderStatus = SD.StatusInProcess;
            return _unitOfWork.Complete();

        }

        public async Task<int> UpdateOrderShipOrder(UpdateOrderView updateOrderView)
        {
            var Header = _unitOfWork._orderHeaderRepository.GetOrder(updateOrderView.Header_Id);
            Header.OrderStatus = SD.StatusShipped;
            Header.Carrier = updateOrderView.Header_Carrier;
            Header.TrackingNumber = updateOrderView.Header_TrackingNumber;
            Header.ShippingDate = DateTime.Now;
            return _unitOfWork.Complete();

        }


        public async Task UpdateOrderCancelOrder(int id)
        {
            var Header = _unitOfWork._orderHeaderRepository.GetByIdAsync(id);
            if (Header.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = Header.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);
                await UpdateStatus(id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                await UpdateStatus(id, SD.StatusCancelled, SD.StatusCancelled);
            }


        }




    }
}
