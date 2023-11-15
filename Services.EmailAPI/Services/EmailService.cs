using Microsoft.EntityFrameworkCore;
using Services.EmailAPI.Data;
using Services.EmailAPI.Message;
using Services.EmailAPI.Models;
using Services.EmailAPI.Models.DTO;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            this.dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDTO cartDTO)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested");
            message.AppendLine("<br/>Total " + cartDTO.CartHeader.CartTotal);
            message.AppendLine("<br/>");
            message.AppendLine("<ul>");
            foreach (var item in cartDTO.CartDetails)
            {
                message.AppendLine("<li>");
                message.AppendLine(item.Product.Name + " x " + item.Count);
                message.AppendLine("</li>");
            }
            message.AppendLine("</ul>");

            await LogAndEmail(message.ToString(), cartDTO.CartHeader.Email);
        }

        public async Task LogOrderPlaced(RewardsMessage rewardsMessage)
        {
            string message = "New Order Placed. <br/> Order ID: " + rewardsMessage.OrderId;
            await LogAndEmail(message, "dotnetmastery@gmail.com");
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = "User reigsterate successful. <br/> Email: " + email;
            await LogAndEmail(message, "dotnetmastery@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
                await using var db = new AppDbContext(dbOptions);
                await db.emailLoggers.AddAsync(emailLog);
                await db.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
