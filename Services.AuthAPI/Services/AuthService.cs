using BackEnd.AuthAPI.Models.DTO;
using BackEnd.CouponAPI.Data;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.AuthAPI.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDTO registerDTO);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<bool> AssignRole(string email, string roleName);
    }
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext context;
        private IJwtTokenGenerate jwtTokenGenerate;
        public AuthService(AppDbContext context, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IJwtTokenGenerate jwtTokenGenerate)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwtTokenGenerate = jwtTokenGenerate;
        }
        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.Email);
            if (user != null)
            {
                var checkPassword = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (checkPassword)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    var token = jwtTokenGenerate.GenerateToken(user, roles);

                    return new LoginResponseDTO
                    {
                        Token = token,
                        Message = "Login Success",
                        User = new UserDTO
                        {
                            Id = user.Id,
                            Email = user.Email,
                            Name = user.UserName,
                            Phone = user.PhoneNumber
                        }
                    };
                }
                return new LoginResponseDTO
                {
                    Message = "Password is incorrect"
                };
            }
            return new LoginResponseDTO
            {
                Message = "Email is not exist"
            };
        }

        public async Task<string> Register(RegisterDTO registerDTO)
        {
            IdentityUser user = new IdentityUser
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.Phone,
            };

            try
            {
                var userExist = await userManager.FindByEmailAsync(registerDTO.Email);
                if (userExist is null)
                {
                    var resultCreate = await userManager.CreateAsync(user, registerDTO.Password);
                    if (resultCreate != null && resultCreate.Succeeded)
                    {
                        return "";
                    }

                    return resultCreate.Errors.First().Description;
                }
                return $"Email {registerDTO.Email} is already exist";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
