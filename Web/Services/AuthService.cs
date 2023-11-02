using Web.Models;
using Web.Services.IServices;
using Web.Utility;

namespace Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService baseService;

        public AuthService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDTO?> AssignRole(RegisterDTO registerDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = registerDTO,
                Url = SD.AuthAPIBase + "/api/auth/AssignRole"
            }, false);
        }

        public async Task<ResponseDTO?> Login(LoginRequestDTO loginRequestDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = loginRequestDTO,
                Url = SD.AuthAPIBase + "/api/auth/Login"
            }, false);
        }

        public async Task<ResponseDTO?> Register(RegisterDTO registerDTO)
        {
            return await baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = registerDTO,
                Url = SD.AuthAPIBase + "/api/auth/Register"
            }, false);
        }
    }
}
