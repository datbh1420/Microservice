using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Web.Models;
using Web.Services.IServices;

namespace Web.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private VnpayOption option;
        public VnPayService(IConfiguration configuration, IOptions<VnpayOption> option)
        {
            _configuration = configuration;
            this.option = option.Value;
        }
        public string CreatePaymentUrl(CartDTO model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(option.TimeZoneId);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var pay = new VnPayLibrary();
            var infor = new
            {
                Name = model.CartHeader.Name,
                Email = model.CartHeader.Email,
                Phone = model.CartHeader.Phone,
                OrderHeaderId = model.CartHeader.CartHeaderId
            };
            pay.AddRequestData("vnp_Version", option.Version);
            pay.AddRequestData("vnp_Command", option.Command);
            pay.AddRequestData("vnp_TmnCode", option.TmnCode);
            pay.AddRequestData("vnp_Amount", ((int)model.CartHeader.CartTotal * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", option.CurrCode);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", option.Locale);
            pay.AddRequestData("vnp_OrderInfo", JsonConvert.SerializeObject(infor));
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", option.Callback);
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
            var paymentUrl =
                pay.CreateRequestUrl(option.BaseUrl, option.HashSecret);

            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, option.HashSecret);

            return response;
        }
    }

}
