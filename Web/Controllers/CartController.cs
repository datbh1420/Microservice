using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;
using Web.Models;
using Web.Services.IServices;
using Web.Utility;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IOrderService orderService;
        private readonly IVnPayService vnPayService;
        private VnpayOption vnpayOption;
        public CartController(ICartService cartService, IOrderService orderService
            , IOptions<VnpayOption> vnpayOption, IVnPayService vnPayService)
        {
            this.cartService = cartService;
            this.orderService = orderService;
            this.vnPayService = vnPayService;
            this.vnpayOption = vnpayOption.Value;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCart());
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
        {
            ResponseDTO? responseDTO = await cartService.ApplyCoupon(cartDTO);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = $"Apply {cartDTO.CartHeader.Code} successful";
            }
            else
            {
                cartDTO.CartHeader.Code = "";
                TempData["error"] = $"{cartDTO.CartHeader.Code} has expired";
            }
            return RedirectToAction("CartIndex");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
        {
            ResponseDTO? responseDTO = await cartService.RemoveCoupon(cartDTO);

            if (responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = $"Remove {cartDTO.CartHeader.Code} successful";
                cartDTO.CartHeader.Code = "";
            }
            return RedirectToAction("CartIndex");
        }

        public async Task<IActionResult> RemoveCart(string cartDetailsId)
        {
            ResponseDTO? responseDTO = await cartService.RemoveCart(cartDetailsId);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = "Delete successful";
            }
            else
            {
                TempData["error"] = responseDTO?.Message;
            }
            return RedirectToAction("CartIndex");
        }

        public async Task<IActionResult> CheckOut()
        {
            return View(await LoadCart());
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CartDTO cartDTO, string paymentMethod)
        {
            CartDTO cart = await LoadCart();
            cart.CartHeader.Email = cartDTO.CartHeader.Email;
            cart.CartHeader.Name = cartDTO.CartHeader.Name;
            cart.CartHeader.Phone = cartDTO.CartHeader.Phone;
            var response = await orderService.CreateOrder(cart);
            OrderHeaderDTO orderHeaderDTO = JsonConvert.DeserializeObject<OrderHeaderDTO>(Convert.ToString(response.Result));
            if (paymentMethod == "Stripe")
            {
                if (orderHeaderDTO != null)
                {
                    var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                    StripeRequestDTO stripeRequestDTO = new StripeRequestDTO
                    {
                        ApproveUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDTO.OrderHeaderId,
                        CancelUrl = domain + "cart/CheckOut",
                        OrderHeader = orderHeaderDTO,
                    };

                    var responseStripe = await orderService.CreateStripeSession(stripeRequestDTO);
                    StripeRequestDTO stripeResult = JsonConvert.DeserializeObject<StripeRequestDTO>(Convert.ToString(responseStripe.Result));
                    Response.Headers.Add("Location", stripeResult.StripeSessionUrl);
                    return new StatusCodeResult(303);
                }
            }
            else if (paymentMethod == "VnPay")
            {
                var url = vnPayService.CreatePaymentUrl(cart, HttpContext);
                return Redirect(url);
            }

            return View();
        }

        public async Task<IActionResult> Confirmation(string orderId)
        {
            ResponseDTO? response = await orderService.ValidateStripeSession(orderId);
            OrderHeaderDTO orderHeaderDTO = JsonConvert.DeserializeObject<OrderHeaderDTO>(Convert.ToString(response.Result));
            if (response is not null && response.IsSuccess)
            {
                if (orderHeaderDTO.Status == SD.Status_Approve)
                {
                    return View(orderHeaderDTO);
                }
            }
            return View(orderHeaderDTO);
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDTO cartDTO)
        {
            CartDTO cart = await LoadCart();
            cart.CartHeader.Email = User.Claims.Where(x => x.Type == ClaimTypes.Email)?.FirstOrDefault()?.Value;

            ResponseDTO? response = await cartService.EmailCart(cart);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Email will be processed and sent shortly.";
            }
            else
            {
                TempData["error"] = "Error";
            }
            return RedirectToAction("CartIndex");
        }
        private async Task<CartDTO> LoadCart()
        {
            var userId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value;
            ResponseDTO? responseDTO = await cartService.GetCart(userId);
            CartDTO cart = new();
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                cart = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(responseDTO.Result));
            }
            return cart;
        }
        public IActionResult PaymentCallback()
        {
            var response = vnPayService.PaymentExecute(Request.Query);
            dynamic orderInfo = JsonConvert.DeserializeObject(response.OrderDescription);
            var result = orderService.UpdateOrderStatus(orderInfo.OrderHeaderId.ToString(), SD.Status_Approve);
            return View(response);
        }
    }
}
