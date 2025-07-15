using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using TranMinhKhoi_com_vn.Entities;

namespace TranMinhKhoi_com_vn.Helper
{
    public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
    {
        private const string HeaderName = "Authorization";
        private const string Prefix = "Apikey ";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var dbContext = context.HttpContext.RequestServices.GetService(typeof(TranMinhKhoiDbContext)) as TranMinhKhoiDbContext;
            if (dbContext == null)
            {
                context.Result = new UnauthorizedObjectResult("Internal server error: cannot access DB context");
                return;
            }

            var expectedApiKey = dbContext.KeySePays.FirstOrDefault()?.KeyApi ?? "KhangAPIKey";

            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var headerValue))
            {
                context.Result = new UnauthorizedObjectResult("Missing Authorization header");
                return;
            }

            if (!headerValue.ToString().StartsWith(Prefix))
            {
                context.Result = new UnauthorizedObjectResult("Invalid Authorization format");
                return;
            }

            var actualApiKey = headerValue.ToString().Substring(Prefix.Length).Trim();

            if (actualApiKey != expectedApiKey)
            {
                context.Result = new UnauthorizedObjectResult("Invalid API Key");
            }
        }
    }
}
