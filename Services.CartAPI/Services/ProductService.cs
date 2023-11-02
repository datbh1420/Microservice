using BackEnd.CartAPI.Models.DTO;
using Newtonsoft.Json;
namespace Services.CartAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>?> GetAllAsync();
    }
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDTO>?> GetAllAsync()
        {
            HttpClient client = httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"api/product");
            var content = await response.Content.ReadAsStringAsync();
            ResponseDTO? responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(content);
            List<ProductDTO>? products = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(responseDTO.Result));
            return products;
        }
    }
}
