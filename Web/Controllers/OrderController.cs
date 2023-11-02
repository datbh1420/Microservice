using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Web.Models;
using Web.Services.IServices;
using Web.Utility;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [Authorize]
        public IActionResult OrderIndex()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> OrderDetail(string orderId)
        {
            OrderHeaderDTO orderHeaderDTO = new();
            string userId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var response = await orderService.GetOrderById(orderId);
            if (response is not null && response.IsSuccess)
            {
                orderHeaderDTO = JsonConvert.DeserializeObject<OrderHeaderDTO>(Convert.ToString(response.Result));
            }
            //CheckUserID
            if (!User.IsInRole(SD.RoleAdmin) && userId != orderHeaderDTO.UserId)
            {
                return NotFound();
            }
            return View(orderHeaderDTO);
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeaderDTO> list;
            string userId = "";
            if (!User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            }
            ResponseDTO response = orderService.GetOrders(userId).GetAwaiter().GetResult();
            if (response is not null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDTO>>(Convert.ToString(response.Result));
                switch (status)
                {
                    case "approved":
                        list = list.Where(x => x.Status == SD.Status_Approve);
                        break;
                    case "readyforpickup":
                        list = list.Where(x => x.Status == SD.Status_ReadyForPickup);
                        break;
                    case "cancelled":
                        list = list.Where(x => x.Status == SD.Status_Cancelled);
                        break;
                    default: break;
                }
            }
            else
            {
                list = new List<OrderHeaderDTO>();
            }
            return Json(new { data = list });
        }


        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(string orderId)
        {
            var response = await orderService.UpdateOrderStatus(orderId, SD.Status_ReadyForPickup);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successful!";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(string orderId)
        {
            var response = await orderService.UpdateOrderStatus(orderId, SD.Status_Completed);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successful!";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            var response = await orderService.UpdateOrderStatus(orderId, SD.Status_Cancelled);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successful!";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }
            return View();
        }
    }
}
