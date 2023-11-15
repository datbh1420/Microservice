using BackEnd.CartAPI.Models.DTO;
using MassTransit;
using MasstTransitRabbitMQ.Contract.Constants;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
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
        private readonly IPublishEndpoint publishEndpoint;
        private ResponseDTO responseDTO;

        public CartAPIController(ICartService cartService, IPublishEndpoint publishEndpoint)
        {
            this.cartService = cartService;
            responseDTO = new ResponseDTO();
            this.publishEndpoint = publishEndpoint;
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

        [HttpPost("EmailCartRequest")]
        public async Task<ResponseDTO> EmailCartRequest([FromBody] CartDTO cartdto)
        {
            try
            {
                await publishEndpoint.Publish(new DomainEvent.EmailCartNotification
                {
                    Id = Guid.NewGuid(),
                    TimeSpan = DateTime.Now,
                    Content = cartdto,
                    Title = "Cart Notification",
                    Type = NotificationType.emailcart
                });
                responseDTO.Result = true;
            }
            catch (Exception ex)
            {
                responseDTO.Message = ex.ToString();
                responseDTO.IsSuccess = false;
            }
            return responseDTO;
        }
    }
}
