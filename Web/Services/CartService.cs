using Web.Models;
using Web.Services.IServices;
using Web.Utility;
namespace Web.Services
{
    public class CartService : ICartService
    {
        private readonly IBaseService baseService;

        public CartService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDTO?> ApplyCoupon(CartDTO cartDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = cartDTO,
                Url = SD.CartAPIBase + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDTO?> GetCart(string userId)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.CartAPIBase + "/api/cart/GetCart/" + userId
            });
        }

        public async Task<ResponseDTO?> RemoveCart(string cartDetailsId)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.CartAPIBase + "/api/cart/RemoveCart/" + cartDetailsId
            });
        }

        public async Task<ResponseDTO?> RemoveCoupon(CartDTO cartDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = cartDTO,
                Url = SD.CartAPIBase + "/api/cart/RemoveCoupon"
            });
        }

        public async Task<ResponseDTO?> UpsertCart(CartDTO cartDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = cartDTO,
                Url = SD.CartAPIBase + "/api/cart/UpsertCart"
            });
        }
    }
}
