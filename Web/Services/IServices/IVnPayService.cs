using Web.Models;

namespace Web.Services.IServices
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(CartDTO model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
