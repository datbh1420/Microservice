using BackEnd.ProductAPI.Models;
using BackEnd.ProductAPI.Models.DTO;
using BackEnd.ProductAPI.Service;
using Microsoft.AspNetCore.Mvc;
using BackEnd.ProductAPI.Models.DTO;

namespace Mango.Service.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        public ResponseDTO response;
        private readonly IProductService service;
        public ProductAPIController(IProductService service)
        {
            response = new ResponseDTO();
            this.service = service;
        }

        [HttpGet]
        public async Task<ResponseDTO> GetAllAsync()
        {
            try
            {
                IEnumerable<Product> objList = await service.GetAllAsync();
                IEnumerable<ProductDTO> objListDto = objList.Select(x => (ProductDTO)x);
                response.Result = objListDto;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ResponseDTO> GetByIdAsync([FromRoute] string Id)
        {
            try
            {
                Product? product = await service.GetByIdAsync(Id);
                if (product is null)
                {
                    response.IsSuccess = false;
                    response.Message = "Product is not exist";
                }
                else
                {
                    response.Result = (ProductDTO)product;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPut]
        public async Task<ResponseDTO> UpdateAsync(ProductDTO productDTO)
        {
            try
            {
                Product? product = await service.UpdateAsync(productDTO);
                if (product is not null)
                {
                    response.Result = (ProductDTO)product;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Product is not Exist";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO> CreateAsync(ProductDTO productDTO)
        {
            try
            {
                Product? product = await service.CreateAsync(productDTO);
                if (product is not null)
                {

                    response.Result = (ProductDTO)product;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Product is already exist";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpDelete]
        [Route("{Id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ResponseDTO> DeleteAsync(string Id)
        {
            try
            {
                Product? product = await service.DeleteByIdAsync(Id);
                if (product is not null)
                {

                    response.Result = (ProductDTO)product;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Product is not Exist";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
