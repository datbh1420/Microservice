using BackEnd.CartAPI.Models.DTO;
using Newtonsoft.Json;

namespace Services.CartAPI.Services
{
    public interface ICouponService
    {
        Task<CouponDTO?> GetCoupon(string Code);
    }
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDTO?> GetCoupon(string Code)
        {
            HttpClient client = httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"api/coupon/{Code}");
            var content = await response.Content.ReadAsStringAsync();
            ResponseDTO? responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(content);
            CouponDTO? coupon = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(responseDTO.Result));

            return coupon;
        }
    }
}
