using static Web.Utility.SD;

namespace Web.Models
{
    public class RequestDTO
    {
        public APIType APIType { get; set; } = APIType.GET;
        public string Url { get; set; } = string.Empty;
        public object? Data { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
