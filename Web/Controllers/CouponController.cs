using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;
using Web.Services.IServices;

namespace Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService couponService;

        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            ResponseDTO? response = await couponService.GetAllAsync();
            List<CouponDTO> list = new();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public IActionResult CreateCoupon()
        {
            CouponDTO couponDTO = new();
            return View(couponDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDTO couponDTO)
        {
            ResponseDTO? response = await couponService.CreateAsync(couponDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Create Successful";
                return RedirectToAction("CouponIndex");
            }
            TempData["error"] = response.Message;
            return View(couponDTO);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCoupon(string Code)
        {
            ResponseDTO? response = await couponService.GetByCode(Code);
            if (ModelState.IsValid)
            {
                if (response != null && response.IsSuccess)
                {
                    CouponDTO? coupon = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
                    return View(coupon);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCoupon(CouponDTO couponDTO)
        {

            if (ModelState.IsValid)
            {
                ResponseDTO? response = await couponService.DeleteAsync(couponDTO.Id);
                if (response is not null && response.IsSuccess)
                {
                    TempData["success"] = "Delete Successfully";
                    return RedirectToAction("CouponIndex");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(couponDTO);
        }

        [HttpGet]
        public async Task<IActionResult> EditCoupon(string Code)
        {
            ResponseDTO? response = await couponService.GetByCode(Code);
            if (ModelState.IsValid)
            {
                if (response != null && response.IsSuccess)
                {
                    CouponDTO? coupon = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
                    return View(coupon);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditCoupon(CouponDTO couponDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await couponService.UpdateAsync(couponDTO);
                if (response is not null && response.IsSuccess)
                {
                    TempData["success"] = "Update Successfully";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(couponDTO);
        }
    }
}
