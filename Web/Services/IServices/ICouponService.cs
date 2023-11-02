using Web.Models;

namespace Web.Services.IServices
{
    public interface ICouponService
    {
        Task<ResponseDTO?> GetAllAsync();
        Task<ResponseDTO?> GetByCode(string code);
        Task<ResponseDTO?> CreateAsync(CouponDTO CouponDTO);
        Task<ResponseDTO?> UpdateAsync(CouponDTO CouponDTO);
        Task<ResponseDTO?> DeleteAsync(string Id);
    }
}
