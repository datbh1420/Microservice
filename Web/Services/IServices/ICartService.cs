using Web.Models;

namespace Web.Services.IServices
{
    public interface ICartService
    {
        Task<ResponseDTO?> GetCart(string userId);
        Task<ResponseDTO?> UpsertCart(CartDTO cartDTO);
        Task<ResponseDTO?> RemoveCart(string cartDetailsId);
        Task<ResponseDTO?> ApplyCoupon(CartDTO cartDTO);
        Task<ResponseDTO?> RemoveCoupon(CartDTO cartDTO);
        Task<ResponseDTO?> EmailCart(CartDTO cartDTO);
    }
}
