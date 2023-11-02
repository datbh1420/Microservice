using Web.Models;

namespace Web.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDTO?> Register(RegisterDTO registerDTO);
        Task<ResponseDTO?> Login(LoginRequestDTO loginRequestDTO);
        Task<ResponseDTO?> AssignRole(RegisterDTO registerDTO);
    }
}
