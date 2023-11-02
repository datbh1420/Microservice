namespace Web.Models
{
    public class VnpayOption
    {
        public string TmnCode { get; set; } = string.Empty;
        public string HashSecret { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string CurrCode { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Locale { get; set; } = string.Empty;
        public string Callback { get; set; } = string.Empty;
        public string TimeZoneId { get; set; } = string.Empty;
    }
}
