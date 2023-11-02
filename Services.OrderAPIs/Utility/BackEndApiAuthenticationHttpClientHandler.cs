﻿using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Services.OrderAPI.Utility
{
    public class BackEndApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public BackEndApiAuthenticationHttpClientHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
