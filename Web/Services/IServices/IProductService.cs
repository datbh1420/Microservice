using Web.Models;

namespace Web.Services.IServices
{
    public interface IProductService
    {
        Task<ResponseDTO?> GetAllAsync();
        Task<ResponseDTO?> GetByIdAsync(string Id);
        Task<ResponseDTO?> CreateAsync(ProductDTO productDTO);
        Task<ResponseDTO?> UpdateAsync(ProductDTO productDTO);
        Task<ResponseDTO?> DeleteAsync(string Id);
    }
}
