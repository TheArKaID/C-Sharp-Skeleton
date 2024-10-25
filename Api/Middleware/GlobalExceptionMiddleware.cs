using Microsoft.AspNetCore.Builder;

namespace RSAHyundai.Api.Middleware
{
    public static class GlobalExceptionMiddleware
    {
        public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
