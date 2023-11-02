using Newtonsoft.Json;
using Services.OrderAPI.Models.DTO;

namespace BackEnd.OrderAPI.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var client = httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/ProductAPI");
            var apiContent = await response.Content.ReadAsStringAsync();
            ResponseDTO? responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            if (responseDTO.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(responseDTO.Result));
            }
            return new List<ProductDTO>();
        }
    }
}
