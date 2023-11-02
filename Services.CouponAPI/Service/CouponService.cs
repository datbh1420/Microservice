using BackEnd.CouponAPI.Data;
using BackEnd.CouponAPI.Models;
using BackEnd.CouponAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.CouponAPI.Service
{
    public interface ICouponService
    {
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon?> GetByCode(string code);
        Task<Coupon?> CreateAsync(CouponDTO CouponDto);
        Task<Coupon?> UpdateAsync(CouponDTO CouponDto);
        Task<Coupon?> DeleteByIdAsync(string Id);
    }

    public class CouponService : ICouponService
    {
        private readonly AppDbContext context;
        public CouponService(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Coupon?> CreateAsync(CouponDTO CouponDto)
        {
            try
            {
                Coupon? CouponExist = await context.coupons.FirstOrDefaultAsync(x => x.Id == CouponDto.Id);
                if (CouponExist is null)
                {
                    Coupon coupon = (Coupon)CouponDto;
                    context.coupons.Add(coupon);
                    await context.SaveChangesAsync();

                    var couponStripe = new Stripe.CouponCreateOptions
                    {
                        AmountOff = (long)coupon.Discount,
                        Name = coupon.Code,
                        Id = coupon.Code,
                        Currency = "vnd"
                    };
                    var service = new Stripe.CouponService();
                    service.Create(couponStripe);

                    return coupon;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Coupon?> DeleteByIdAsync(string Id)
        {
            try
            {
                Coupon? CouponExist = await context.coupons.FirstOrDefaultAsync(x => x.Id == Id);
                if (CouponExist is not null)
                {
                    context.coupons.Remove(CouponExist);
                    await context.SaveChangesAsync();

                    var service = new Stripe.CouponService();
                    service.Delete(CouponExist.Code);
                    return CouponExist;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            try
            {
                IEnumerable<Coupon> list = await context.coupons.ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Coupon?> GetByCode(string code)
        {
            try
            {
                Coupon? entity = await context.coupons.FirstOrDefaultAsync(x => x.Code == code);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Coupon?> UpdateAsync(CouponDTO CouponDto)
        {
            try
            {
                Coupon? CouponExist = await context.coupons.FirstOrDefaultAsync(x => x.Id == CouponDto.Id);
                if (CouponExist is not null)
                {
                    Coupon coupon = (Coupon)CouponDto;
                    context.coupons.Update(coupon);
                    await context.SaveChangesAsync();

                    var service = new Stripe.CouponService();
                    var couponStripe = new Stripe.CouponCreateOptions
                    {
                        AmountOff = (long)coupon.Discount,
                        Name = coupon.Code,
                        Id = coupon.Code,
                        Currency = "vnd"
                    };
                    service.Delete(CouponExist.Code);
                    service.Create(couponStripe);
                    return coupon;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
