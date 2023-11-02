using Facebook;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Web.Models;
using Web.Services.IServices;
using Web.Utility;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly ITokenProvider tokenProvider;
        private IHttpClientFactory httpClientFactory;
        private GoogleOption googleOption;
        private FaceBookOption faceBookOption;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider,
            IOptions<GoogleOption> googleOption, IOptions<FaceBookOption> faceBookOption,
            IHttpClientFactory httpClientFactory)
        {
            this.authService = authService;
            this.tokenProvider = tokenProvider;
            this.faceBookOption = faceBookOption.Value;
            this.googleOption = googleOption.Value;
            this.httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public IActionResult Login()
        {
            var fbClient = new FacebookClient();
            var fbUrl = fbClient.GetLoginUrl(new
            {
                client_id = faceBookOption.AppId,
                redirect_uri = faceBookOption.RedirectUri,
                scope = "public_profile,email"
            });
            ViewBag.FaceBookUrl = fbUrl;
            ViewBag.GoogleUrl = "https://accounts.google.com/o/oauth2/auth" +
                "?scope=profile" +
                "&redirect_uri=" + googleOption.RedirectUri +
                "&response_type=code" +
                "&client_id=" + googleOption.ClientId +
                "&approval_prompt=force";
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            ResponseDTO? response = await authService.Login(loginRequestDTO);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDTO? loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(
                    Convert.ToString(response.Result.ToString()));

                //SignIn: Create Identity
                await SignInAsync(loginResponse);
                //SetToken in Cookie
                tokenProvider.SetToken(loginResponse.Token);
                TempData["success"] = $"Hi {loginResponse.User?.Name}";
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = response.Message;
            return View(loginRequestDTO);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterDTO registerDTO = new RegisterDTO();
            var listRole = new List<SelectListItem?>{
                new SelectListItem { Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem { Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };
            ViewBag.ListRole = listRole;
            return View(registerDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            ResponseDTO? response = await authService.Register(registerDTO);
            if (response != null && response.IsSuccess)
            {
                if (!string.IsNullOrEmpty(registerDTO.Role))
                {
                    ResponseDTO responseAssignRole = await authService.AssignRole(registerDTO);
                    if (responseAssignRole != null && responseAssignRole.IsSuccess)
                    {
                        TempData["success"] = "Register successful";
                        return RedirectToAction("Login");
                    }
                }
                TempData["error"] = "Role is not exist";
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return View(registerDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            tokenProvider.ClearToken();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInAsync(LoginResponseDTO loginResponse)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponse.Token);
            //Create Identity
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier,
                jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            identity.AddClaim(new Claim(ClaimTypes.Email,
                jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));

            foreach (var roleClaim in jwt.Claims.Where(x => x.Type == ClaimTypes.Role))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
            }

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }




        public async Task<IActionResult> Google()
        {
            string code = Request.Query["code"];
            string accessToken = await getToken(code);
            RegisterDTO registerDTO = await getUser(accessToken);
            return RedirectToAction("LoginOthers", registerDTO);
        }

        private async Task<RegisterDTO> getUser(string accessToken)
        {
            var client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync(googleOption.LinkGetUserInfor);
            var contentResponse = await response.Content.ReadAsStringAsync();

            dynamic userInfoJson = JsonConvert.DeserializeObject(contentResponse);

            RegisterDTO userJson = new RegisterDTO
            {
                UserName = userInfoJson.name,
                Email = userInfoJson.id,
                Phone = "0123456789",
                Password = "Abcd123@",
                Role = SD.RoleCustomer,
            };
            return userJson;
        }

        private async Task<string> getToken(string? code)
        {
            var client = httpClientFactory.CreateClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", googleOption.ClientId),
                new KeyValuePair<string, string>("client_secret", googleOption.ClientSecret),
                new KeyValuePair<string, string>("redirect_uri", googleOption.RedirectUri),
                new KeyValuePair<string, string>("grant_type", googleOption.GrantType)
            });

            var response = await client.PostAsync(googleOption.LinkGetToken, content);
            var contentResponse = await response.Content.ReadAsStringAsync();

            dynamic token = JsonConvert.DeserializeObject(contentResponse);
            return token.access_token;
        }


        public IActionResult FaceBook(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("oauth/access_token", new
            {
                client_id = faceBookOption.AppId,
                client_secret = faceBookOption.AppSecret,
                redirect_uri = faceBookOption.RedirectUri,
                code = code
            });

            fb.AccessToken = result.access_token;

            dynamic me = fb.Get("/me?fields=name,email");

            RegisterDTO registerDTO = new RegisterDTO
            {
                UserName = Guid.NewGuid().ToString().Substring(0, 8),
                Email = me.id,
                Phone = "0123456789",
                Password = "Abcd123@",
                Role = SD.RoleCustomer
            };

            return RedirectToAction("LoginOthers", registerDTO);
        }

        public async Task<IActionResult> LoginOthers(RegisterDTO registerDTO)
        {
            ResponseDTO? responseRegister = await authService.Register(registerDTO);
            if (responseRegister != null && responseRegister.IsSuccess)
            {
                if (!string.IsNullOrEmpty(registerDTO.Role))
                {
                    ResponseDTO responseAssignRole = await authService.AssignRole(registerDTO);
                }
                TempData["error"] = "Role is not exist";
            }

            LoginRequestDTO loginRequestDTO = new LoginRequestDTO
            {
                Email = registerDTO.Email,
                Password = registerDTO.Password,
            };
            ResponseDTO? responseLogin = await authService.Login(loginRequestDTO);
            if (responseLogin != null && responseLogin.IsSuccess)
            {
                LoginResponseDTO? loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(
                    Convert.ToString(responseLogin.Result.ToString()));

                //SignIn: Create Identity
                await SignInAsync(loginResponse);
                //SetToken in Cookie
                tokenProvider.SetToken(loginResponse.Token);
                TempData["success"] = $"Hi {loginResponse.User?.Name}";
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = responseLogin.Message;
            return RedirectToAction("Login");
        }
    }
}
