using AutoMapper;
using BackEnd.Order.Data;
using Microsoft.EntityFrameworkCore;
using Services.OrderAPI.Models;
using Services.OrderAPI.Models.DTO;
using Services.OrderAPI.Utility;
using Stripe;
using Stripe.Checkout;

namespace Services.OrderAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderHeaderDTO>> GetAllByAdmin();
        Task<IEnumerable<OrderHeaderDTO>> GetAllByCustomer(string userId);
        Task<OrderHeaderDTO?> GetByIdAsync(string Id);
        Task<OrderHeaderDTO?> CreateAsync(CartDTO cartDTO);
        Task<StripeRequestDTO> CreateStripeSession(StripeRequestDTO stripeRequestDTO);
        Task<OrderHeader> ValidateStripeSession(string orderHeaderId);
        Task<bool> UpdateOrderStatus(string orderId, string newStatus);
    }
    public class OrderService : IOrderService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public OrderService(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<OrderHeaderDTO?> CreateAsync(CartDTO cartDTO)
        {
            try
            {
                OrderHeaderDTO orderHeaderDTO = mapper.Map<OrderHeaderDTO>(cartDTO.CartHeader);
                orderHeaderDTO.OrderTime = DateTime.Now;
                orderHeaderDTO.Status = SD.Status_Pending;
                orderHeaderDTO.OrderDetails = mapper.Map<IEnumerable<OrderDetailsDTO>>(cartDTO.CartDetails);

                OrderHeader orderHeader = context.orderHeaders.Add(mapper.Map<OrderHeader>(orderHeaderDTO)).Entity;
                await context.SaveChangesAsync();

                return (OrderHeaderDTO)orderHeader;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StripeRequestDTO> CreateStripeSession(StripeRequestDTO stripeRequestDTO)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDTO.ApproveUrl,
                    CancelUrl = stripeRequestDTO.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment"
                };

                //AddDiscount
                if (stripeRequestDTO.OrderHeader.Discount > 0)
                {
                    options.Discounts = new List<SessionDiscountOptions>
                    {
                        new SessionDiscountOptions
                        {
                            Coupon = stripeRequestDTO.OrderHeader.Code
                        }
                    };
                }

                //AddItemInStripeSession
                foreach (var item in stripeRequestDTO.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        Quantity = item.Count,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)item.Price * item.Count,
                            Currency = "vnd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Images = new List<string> { item.Product.ImagePath },
                                Description = item.Product.Description,
                                Name = item.Product.Name,
                            }
                        }
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                //CreateSession
                SessionService service = new SessionService();
                Session session = service.Create(options);

                //Update
                stripeRequestDTO.StripeSessionUrl = session.Url;
                stripeRequestDTO.StripeSessionId = session.Id;

                OrderHeader orderHeader = await context.orderHeaders.FirstAsync(x => x.OrderHeaderId == stripeRequestDTO.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                context.orderHeaders.Update(orderHeader);
                await context.SaveChangesAsync();

                return stripeRequestDTO;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderHeaderDTO>> GetAllByAdmin()
        {
            try
            {
                IEnumerable<OrderHeader> list = await context.orderHeaders.Include(x => x.OrderDetails)
                    .OrderByDescending(x => x.OrderHeaderId).ToListAsync();
                var listDTo = list.Select(x => (OrderHeaderDTO)x);
                return listDTo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderHeaderDTO>> GetAllByCustomer(string userId)
        {
            try
            {
                IEnumerable<OrderHeader> list = await context.orderHeaders.Include(x => x.OrderDetails)
                    .Where(x => x.UserId == userId).OrderByDescending(x => x.OrderHeaderId).ToListAsync();
                var listDTo = list.Select(x => (OrderHeaderDTO)x);
                return listDTo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderHeaderDTO?> GetByIdAsync(string Id)
        {
            try
            {
                OrderHeader orderHeader = await context.orderHeaders.Include(x => x.OrderDetails).FirstAsync(x => x.OrderHeaderId == Id);
                return (OrderHeaderDTO?)orderHeader;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateOrderStatus(string orderId, string newStatus)
        {
            try
            {
                OrderHeader orderHeader = await context.orderHeaders.FirstAsync(x => x.OrderHeaderId == orderId);
                if (newStatus == SD.Status_Cancelled)
                {
                    var options = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderHeader.PaymentIntentId
                    };

                    RefundService service = new();
                    Refund refund = service.Create(options);
                }

                orderHeader.Status = newStatus;
                context.orderHeaders.Update(orderHeader);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderHeader> ValidateStripeSession(string orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = await context.orderHeaders.FirstAsync(x => x.OrderHeaderId == orderHeaderId);

                SessionService service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);

                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    //then payment was successful
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = SD.Status_Approve;
                    context.orderHeaders.Update(orderHeader);
                    context.SaveChanges();


                }
                return orderHeader;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
