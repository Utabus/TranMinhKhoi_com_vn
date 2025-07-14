using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TranMinhKhoi_com_vn.Helper
{
    public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
    {
        private const string HeaderName = "Authorization";
        private const string Prefix = "Apikey ";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var expectedApiKey = "KhangAPIKey";

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
