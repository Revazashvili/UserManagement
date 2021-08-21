using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class UrlService : IUrlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GenerateEmailConfirmationLink(string userId, string token)
        {
            const string prefix = "api";
            const string controllerName = "User"; 
            const string actionName = "VerifyEmail";
            var link =
                $"{_httpContextAccessor?.HttpContext?.Request.Scheme}//{_httpContextAccessor?.HttpContext?.Request.Host.ToString()}" +
                $"/{prefix}/{controllerName}/{actionName}?userId={userId}&token={token}";
            return link;
        }
    }
}