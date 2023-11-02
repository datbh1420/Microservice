using Web.Models;
using Web.Services.IServices;
using Web.Utility;

namespace Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService baseService;

        public OrderService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDTO?> CreateOrder(CartDTO cartDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = cartDTO,
                Url = SD.OrderAPIBase + "/api/order/CreateOrder",
            });
        }

        public async Task<ResponseDTO?> CreateStripeSession(StripeRequestDTO stripeRequestDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = stripeRequestDTO,
                Url = SD.OrderAPIBase + "/api/order/CreateStripeSession",
            });
        }

        public async Task<ResponseDTO?> GetOrderById(string Id)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Data = Id,
                Url = SD.OrderAPIBase + "/api/order/GetOrderById"
            });
        }

        public async Task<ResponseDTO?> GetOrders(string? userId = "")
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Data = userId,
                Url = SD.OrderAPIBase + "/api/order/GetOrders"
            });
        }

        public async Task<ResponseDTO?> UpdateOrderStatus(string orderId, string newStatus)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = newStatus,
                Url = SD.OrderAPIBase + "/api/order/UpdateOrderStatus/" + orderId
            });
        }

        public async Task<ResponseDTO?> ValidateStripeSession(string orderHeaderId)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.OrderAPIBase + "/api/order/ValidateStripeSession/" + orderHeaderId
            });
        }
    }
}
