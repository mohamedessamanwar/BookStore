using BookStore.BusnessLogic.ViewsModels.Order;
using BookStore.DataAccessLayer.Models;

namespace BookStore.BusnessLogic.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderConfirmation> CreateOrder(OrderSummary orderSummary, string userid);

        Task<OrderSummary> Summary(string userid);
        Task<OrderHeader> GetOrderAsync(int id);
        Task UpdateStripePaymentId(int id, string SessionId, string PaymentIntentId);
        Task UpdateStatus(int id, string statusApproved, string paymentStatusApproved);
        Task<List<OrderTableView>> GetOrders(string status);
        Task<OrderView> GetDetails(int id);
        Task<List<OrderTableView>> GetUserOrders(string status, string id);
        Task<int> UpdateOrder(int id, UpdateOrderView updateOrderView);
        Task<int> UpdateOrderStartProcessing(int id);
        Task<int> UpdateOrderShipOrder(UpdateOrderView updateOrderView);

        Task UpdateOrderCancelOrder(int id);


    }
}
