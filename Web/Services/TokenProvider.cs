using Web.Services.IServices;
using Web.Utility;

namespace Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(SD.TokenCookie);
        }

        public string? GetToken()
        {
            string? token = null;
            bool hasToken = httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.TokenCookie, out token);
            return hasToken ? token : "";
        }

        public void SetToken(string token)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Append(SD.TokenCookie, token);
        }
    }
}
