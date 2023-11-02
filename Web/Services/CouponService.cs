using Web.Models;
using Web.Services.IServices;
using Web.Utility;

namespace Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService baseService;

        public CouponService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDTO?> CreateAsync(CouponDTO CouponDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = CouponDTO,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTO?> DeleteAsync(string Id)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.CouponAPIBase + "/api/coupon/" + Id
            });
        }

        public async Task<ResponseDTO?> GetAllAsync()
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTO?> GetByCode(string code)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/" + code
            });
        }

        public async Task<ResponseDTO?> UpdateAsync(CouponDTO CouponDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = CouponDTO,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }
    }
}
