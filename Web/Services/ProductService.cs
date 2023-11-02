using Web.Models;
using Web.Services.IServices;

namespace Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService baseService;

        public ProductService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDTO?> CreateAsync(ProductDTO productDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = Utility.SD.APIType.POST,
                Url = Utility.SD.ProductAPIBase + "/api/product",
                Data = productDTO,
                ContentType = Utility.SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDTO?> DeleteAsync(string Id)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = Utility.SD.APIType.DELETE,
                Url = Utility.SD.ProductAPIBase + "/api/product/" + Id,
            });
        }

        public async Task<ResponseDTO?> GetAllAsync()
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = Utility.SD.APIType.GET,
                Url = Utility.SD.ProductAPIBase + "/api/product",
            });
        }

        public async Task<ResponseDTO?> GetByIdAsync(string Id)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = Utility.SD.APIType.GET,
                Url = Utility.SD.ProductAPIBase + "/api/product/" + Id,
            });
        }

        public async Task<ResponseDTO?> UpdateAsync(ProductDTO productDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = Utility.SD.APIType.PUT,
                Url = Utility.SD.ProductAPIBase + "/api/product",
                Data = productDTO,
                ContentType = Utility.SD.ContentType.MultipartFormData
            });
        }
    }
}
