using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Teapot.Web;

internal static class FairUseRateLimiterExtensions
{
    internal const string PolicyName = nameof(FairUseRateLimiterPolicy);
    public static IServiceCollection AddFairUseRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy<string, FairUseRateLimiterPolicy>(PolicyName);
        });

        return services;
    }

    public static WebApplication UseFairUseRateLimiter(this WebApplication app)
    {
        app.UseRateLimiter();

        return app;
    }
}
