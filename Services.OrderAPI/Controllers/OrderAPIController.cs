using MassTransit;
using MasstTransitRabbitMQ.Contract.Constants;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
using Microsoft.AspNetCore.Mvc;
using Services.OrderAPI.Models;
using Services.OrderAPI.Models.DTO;
using Services.OrderAPI.Services;
using Services.OrderAPI.Utility;

namespace Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]

    public class OrderAPIController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IPublishEndpoint publishEndpoint;
        private ResponseDTO responseDTO;

        public OrderAPIController(IOrderService orderService, IPublishEndpoint publishEndpoint)
        {
            this.orderService = orderService;
            responseDTO = new ResponseDTO();
            this.publishEndpoint = publishEndpoint;
        }
        [HttpGet("GetOrders")]
        public async Task<ResponseDTO> GetOrders(string? userId = "")
        {
            try
            {
                IEnumerable<OrderHeaderDTO> orderList;
                if (User.IsInRole(SD.RoleAdmin))
                {
                    orderList = await orderService.GetAllByAdmin();
                }
                else
                {
                    orderList = await orderService.GetAllByCustomer(userId);
                }
                responseDTO.Result = orderList;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }
        [HttpGet("GetOrderById")]
        public async Task<ResponseDTO> GetOrderById(string Id)
        {
            try
            {
                OrderHeaderDTO? orderHeaderDTO = await orderService.GetByIdAsync(Id);
                if (orderHeaderDTO != null)
                {
                    responseDTO.Result = orderHeaderDTO;
                }
                else
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Order is not exist";
                }
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }
        [HttpPost("CreateOrder")]
        public async Task<ResponseDTO> CreateOrder(CartDTO cartDTO)
        {
            try
            {
                OrderHeaderDTO? orderHeaderDTO = await orderService.CreateAsync(cartDTO);
                responseDTO.Result = orderHeaderDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDTO> CreateStripeSession(StripeRequestDTO stripeRequestDTO)
        {
            try
            {
                stripeRequestDTO = await orderService.CreateStripeSession(stripeRequestDTO);
                responseDTO.Result = stripeRequestDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }
        [HttpPost("ValidateStripeSession/{orderHeaderId}")]
        public async Task<ResponseDTO> ValidateStripeSession(string orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = await orderService.ValidateStripeSession(orderHeaderId);
                RewardsDTO rewardsDTO = new()
                {
                    OrderId = orderHeader.OrderHeaderId,
                    RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal),
                    UserId = orderHeader.UserId
                };
                await publishEndpoint.Publish(new DomainEvent.RewardsNotification
                {
                    Id = Guid.NewGuid(),
                    TimeSpan = DateTime.Now,
                    Content = rewardsDTO,
                    Title = "Rewards Notification",
                    Type = NotificationType.rewards
                });
                responseDTO.Result = orderHeader;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }
        [HttpPost("UpdateOrderStatus/{orderId}")]
        public async Task<ResponseDTO> UpdateOrderStatus(string orderId, [FromBody] string newStatus)
        {
            try
            {
                bool result = await orderService.UpdateOrderStatus(orderId, newStatus);
                responseDTO.Result = result;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

    }
}
