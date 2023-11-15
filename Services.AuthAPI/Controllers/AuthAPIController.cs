using BackEnd.AuthAPI.Models.DTO;
using BackEnd.AuthAPI.Services;
using MassTransit;
using MasstTransitRabbitMQ.Contract.Constants;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService service;
        private ResponseDTO responseDTO;
        private IPublishEndpoint publishEndpoint;
        public AuthAPIController(IAuthService service, IPublishEndpoint publishEndpoint)
        {
            this.service = service;
            responseDTO = new ResponseDTO();
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost("Register")]
        public async Task<ResponseDTO> Register(RegisterDTO registerDTO)
        {
            try
            {
                var result = await service.Register(registerDTO);
                if (string.IsNullOrEmpty(result))
                {
                    await publishEndpoint.Publish(new DomainEvent.RegisterNotification
                    {
                        Id = Guid.NewGuid(),
                        TimeSpan = DateTime.Now,
                        Content = result,
                        Title = "Register Notification",
                        Type = NotificationType.register
                    });

                    return responseDTO;
                }
                responseDTO.IsSuccess = false;
                responseDTO.Message = result;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        [HttpPost("Login")]
        public async Task<ResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var result = await service.Login(loginRequestDTO);
                if (result.User != null)
                {
                    responseDTO.Result = new
                    {
                        User = result.User,
                        Token = result.Token
                    };
                    responseDTO.Message = result.Message;
                    return responseDTO;
                }
                responseDTO.IsSuccess = false;
                responseDTO.Message = result.Message;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        [HttpPost("AssignRole")]
        public async Task<ResponseDTO> AssignRole(RegisterDTO registerDTO)
        {
            var result = await service.AssignRole(registerDTO.Email, registerDTO.Role);
            if (result)
            {
                responseDTO.IsSuccess = true;
                responseDTO.Message = "Assign Success";
                return responseDTO;
            }
            responseDTO.IsSuccess = false;
            responseDTO.Message = "Error";
            return responseDTO;
        }
    }
}
