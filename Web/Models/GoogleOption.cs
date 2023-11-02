namespace Web.Models
{
    public class GoogleOption
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string LinkGetToken { get; set; } = string.Empty;
        public string LinkGetUserInfor { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;
    }
}
