using Microsoft.AspNetCore.Mvc;
using BackEnd.AuthAPI.Models.DTO;
using BackEnd.AuthAPI.Services;

namespace BackEnd.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService service;
        private ResponseDTO responseDTO;
        public AuthAPIController(IAuthService service)
        {
            this.service = service;
            responseDTO = new ResponseDTO();
        }

        [HttpPost("Register")]
        public async Task<ResponseDTO> Register(RegisterDTO registerDTO)
        {
            try
            {
                var result = await service.Register(registerDTO);
                if (!string.IsNullOrEmpty(result))
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = result;
                }
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
