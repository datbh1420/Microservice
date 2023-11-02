using BackEnd.CartAPI.Data;
using BackEnd.CartAPI.Models;
using BackEnd.CartAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Services.CartAPI.Models.DTO;

namespace Services.CartAPI.Services
{
    public interface ICartService
    {
        Task<CartDTO> GetCart(string userId);
        Task<bool> UpsertCart(CartDTO cartDTO);
        Task<bool> RemoveCart(string cartDetailsId);
        Task<bool> ApplyCoupon(CartDTO cartDTO);
        Task<bool> RemoveCoupon(CartDTO cartDTO);
    }
    public class CartService : ICartService
    {
        private readonly AppDbContext context;
        private readonly IProductService productService;
        private readonly ICouponService couponService;
        public CartService(AppDbContext context, IProductService productService,
            ICouponService couponService)
        {
            this.context = context;
            this.productService = productService;
            this.couponService = couponService;
        }
        public async Task<bool> ApplyCoupon(CartDTO cartDTO)
        {
            try
            {
                CouponDTO? coupon = await couponService.GetCoupon(cartDTO.CartHeader.Code);
                if (coupon != null)
                {
                    var cartHeader = context.cartHeaders.First(x => x.CartHeaderId == cartDTO.CartHeader.CartHeaderId);
                    cartHeader.Code = cartDTO.CartHeader.Code;
                    context.cartHeaders.Update(cartHeader);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CartDTO> GetCart(string userId)
        {
            try
            {
                CartDTO cartDTO = new();
                CartHeader cartHeader = await context.cartHeaders.FirstAsync(x => x.UserId == userId);
                cartDTO.CartHeader = (CartHeaderDTO)cartHeader;

                var cartDetails = context.cartDetails.Where(x => x.CartHeaderId == cartHeader.CartHeaderId).ToList();

                cartDTO.CartDetails = cartDetails.Select(x => (CartDetailsDTO)x).ToList();

                IEnumerable<ProductDTO> productDTOs = await productService.GetAllAsync();
                foreach (var item in cartDTO.CartDetails)
                {
                    var pro = productDTOs.FirstOrDefault(x => x.Id == item.ProductId);
                    item.Product = pro;
                    cartDTO.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }


                //Discount
                if (!string.IsNullOrEmpty(cartDTO.CartHeader.Code))
                {
                    CouponDTO? coupon = await couponService.GetCoupon(cartDTO.CartHeader.Code);
                    if (coupon != null)
                    {
                        cartDTO.CartHeader.Discount = coupon.Discount;
                        cartDTO.CartHeader.CartTotal -= coupon.Discount;
                    }
                }

                return cartDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveCart(string cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = context.cartDetails.First(u => u.CartDetailsId == cartDetailsId);
                var totalCountOfCartItem = context.cartDetails.Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();
                if (totalCountOfCartItem == 1)
                {
                    var cartHeader = context.cartHeaders.Find(cartDetails.CartHeaderId);
                    context.cartHeaders.Remove(cartHeader);
                }
                context.cartDetails.Remove(cartDetails);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveCoupon(CartDTO cartDTO)
        {
            try
            {
                var cartHeader = context.cartHeaders.First(x => x.CartHeaderId == cartDTO.CartHeader.CartHeaderId);
                cartHeader.Code = "";
                context.cartHeaders.Update(cartHeader);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpsertCart(CartDTO cartDTO)
        {
            try
            {
                var cartHeaderExist = await context.cartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cartDTO.CartHeader.UserId);
                if (cartHeaderExist == null)
                {
                    context.cartHeaders.Add((CartHeader)cartDTO.CartHeader);
                    await context.SaveChangesAsync();

                    foreach (var item in cartDTO.CartDetails)
                    {
                        item.CartHeaderId = cartDTO.CartHeader.CartHeaderId;
                        context.cartDetails.Add((CartDetails)item);
                    }
                    await context.SaveChangesAsync();
                }
                else
                {

                    var productExistInCart = await context.cartDetails.FirstOrDefaultAsync(x =>
                    x.CartHeaderId == cartHeaderExist.CartHeaderId &&
                    x.ProductId == cartDTO.CartDetails.First().ProductId);

                    if (productExistInCart == null)
                    {
                        cartDTO.CartDetails.First().CartHeaderId = cartHeaderExist.CartHeaderId;
                        var cartAdd = cartDTO.CartDetails.First();
                        context.cartDetails.Add((CartDetails)cartAdd);
                    }
                    else
                    {
                        productExistInCart.Count += cartDTO.CartDetails.First().Count;
                        context.cartDetails.Update(productExistInCart);
                    }
                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
