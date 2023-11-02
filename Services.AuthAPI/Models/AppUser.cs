using Microsoft.AspNetCore.Identity;

namespace Services.AuthAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
