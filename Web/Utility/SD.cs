namespace Web.Utility
{
    public class SD
    {
        public static string AuthAPIBase { get; set; } = "";
        public static string ProductAPIBase { get; set; } = "";
        public static string CouponAPIBase { get; set; } = "";
        public static string CartAPIBase { get; set; } = "";
        public static string OrderAPIBase { get; set; } = "";
        public static string TokenCookie { get; set; } = "JwtToken";
        public static string RoleAdmin { get; set; } = "Admin";
        public static string RoleCustomer { get; set; } = "Customer";
        public enum APIType
        {
            GET, POST, PUT, DELETE
        }
        public enum ContentType
        {
            Json,
            MultipartFormData
        }

        public const string Status_Pending = "Pending";
        public const string Status_Approve = "Approved";
        public const string Status_ReadyForPickup = "ReadyForPickup";
        public const string Status_Completed = "Complete";
        public const string Status_Refunded = "Refunded";
        public const string Status_Cancelled = "Cancelled";
    }
}
