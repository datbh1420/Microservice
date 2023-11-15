using BackEnd.CouponAPI.Models;
using BackEnd.CouponAPI.Models.DTO;
using BackEnd.CouponAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    //[Authorize]
    public class CouponAPIController : ControllerBase
    {
        public ResponseDTO response;
        private readonly ICouponService service;
        public CouponAPIController(ICouponService service)
        {
            response = new ResponseDTO();
            this.service = service;
        }

        [HttpGet]
        public async Task<ResponseDTO> GetAllAsync()
        {
            try
            {
                IEnumerable<Coupon> objList = await service.GetAllAsync();
                IEnumerable<CouponDTO> objListDto = objList.Select(x => (CouponDTO)x);
                response.Result = objListDto;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet("{Code}")]
        public async Task<ResponseDTO> GetByCode(string Code)
        {
            try
            {
                Coupon? coupon = await service.GetByCode(Code);
                if (coupon == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Coupon is not exist";
                    return response;
                }
                response.Result = coupon;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpPut]
        public async Task<ResponseDTO> UpdateAsync(CouponDTO CouponDTO)
        {
            try
            {
                Coupon? Coupon = await service.UpdateAsync(CouponDTO);
                if (Coupon is not null)
                {
                    response.Result = (CouponDTO)Coupon;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Coupon is not Exist";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO> CreateAsync(CouponDTO CouponDTO)
        {
            try
            {
                Coupon? Coupon = await service.CreateAsync(CouponDTO);
                if (Coupon is not null)
                {

                    response.Result = (CouponDTO)Coupon;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Coupon is already exist";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpDelete]
        [Route("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDTO> DeleteAsync(string Id)
        {
            try
            {
                Coupon? Coupon = await service.DeleteByIdAsync(Id);
                if (Coupon is not null)
                {

                    response.Result = (CouponDTO)Coupon;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Coupon is not Exist";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
