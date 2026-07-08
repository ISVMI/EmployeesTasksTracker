using Microsoft.AspNetCore.Builder;
using Shared.Middleware;

namespace Shared.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            return app;
        }
    }
}
