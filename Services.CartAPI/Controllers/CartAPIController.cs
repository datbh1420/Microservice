using BackEnd.CartAPI.Models.DTO;


using Microsoft.AspNetCore.Mvc;
using Services.CartAPI.Models.DTO;
using Services.CartAPI.Services;
namespace Services.CartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly ICartService cartService;
        private ResponseDTO responseDTO;

        public CartAPIController(ICartService cartService)
        {
            this.cartService = cartService;
            responseDTO = new ResponseDTO();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDTO> GetCart(string userId)
        {
            CartDTO cart = await cartService.GetCart(userId);
            responseDTO.Result = cart;
            return responseDTO;
        }

        [HttpPost("UpsertCart")]
        public async Task<ResponseDTO> UpsertCart(CartDTO cartDTO)
        {
            var result = await cartService.UpsertCart(cartDTO);
            responseDTO.Result = result;
            return responseDTO;
        }
        [HttpPost("RemoveCart/{cartDetailsId}")]
        public async Task<ResponseDTO> RemoveCart(string cartDetailsId)
        {
            var result = await cartService.RemoveCart(cartDetailsId);
            responseDTO.Result = result;
            return responseDTO;
        }
        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDTO> ApplyCoupon(CartDTO cartDTO)
        {
            var result = await cartService.ApplyCoupon(cartDTO);
            responseDTO.IsSuccess = result;
            return responseDTO;
        }
        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDTO> RemoveCoupon(CartDTO cartDTO)
        {
            var result = await cartService.RemoveCoupon(cartDTO);
            responseDTO.Result = result;
            return responseDTO;
        }
    }
}
