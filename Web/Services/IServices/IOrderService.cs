using Web.Models;

namespace Web.Services.IServices
{
    public interface IOrderService
    {
        Task<ResponseDTO?> GetOrders(string? userId = "");
        Task<ResponseDTO?> GetOrderById(string Id);
        Task<ResponseDTO?> CreateOrder(CartDTO cartDTO);
        Task<ResponseDTO?> CreateStripeSession(StripeRequestDTO stripeRequestDTO);
        Task<ResponseDTO?> ValidateStripeSession(string orderHeaderId);
        Task<ResponseDTO?> UpdateOrderStatus(string orderId, string newStatus);
    }
}
