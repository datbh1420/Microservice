﻿using Services.EmailAPI.Models.DTO;

namespace Mango.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDTO cartDTO);
        Task RegisterUserEmailAndLog(string email);
        Task LogOrderPlaced(RewardsDTO rewardsDTO);
    }
}
