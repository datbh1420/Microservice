using Newtonsoft.Json;
using System.Net;
using System.Text;
using Web.Models;
using Web.Services.IServices;
using static Web.Utility.SD;

namespace Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ITokenProvider tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            this.httpClientFactory = httpClientFactory;
            this.tokenProvider = tokenProvider;
        }

        public async Task<ResponseDTO?> SendAsync(RequestDTO request, bool withBearer = true)
        {
            HttpClient client = httpClientFactory.CreateClient();
            HttpRequestMessage message = new HttpRequestMessage();

            switch (request.APIType)
            {
                case APIType.POST:
                    message.Method = HttpMethod.Post; break;
                case APIType.PUT:
                    message.Method = HttpMethod.Put; break;
                case APIType.DELETE:
                    message.Method = HttpMethod.Delete; break;
                default: message.Method = HttpMethod.Get; break;
            }
            if (request.ContentType == ContentType.Json)
            {
                message.Headers.Add("Accept", "application/json");
            }
            else
            {
                message.Headers.Add("Accept", "*/*");
            }

            message.RequestUri = new Uri(request.Url);

            if (withBearer)
            {
                var token = tokenProvider.GetToken();
                message.Headers.Add("Authorization", $"Bearer {token}");
            }

            if (request.Data != null)
            {
                if (request.ContentType == ContentType.Json)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data)
                        , Encoding.UTF8, "application/json");
                }
                else
                {
                    var content = new MultipartFormDataContent();
                    foreach (var item in request.Data.GetType().GetProperties())
                    {
                        var value = item.GetValue(request.Data);

                        if (value is IFormFile)
                        {
                            var file = (IFormFile)value;
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), item.Name, file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value is null ? "" : value.ToString(), Encoding.UTF8));
                        }
                    }
                    message.Content = content;
                }
            }

            HttpResponseMessage response = await client.SendAsync(message);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access Denied" };
                case HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Error" };
                default:
                    var content = await response.Content.ReadAsStringAsync();
                    var responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(content);
                    return responseDTO;
            }
        }
    }
}
