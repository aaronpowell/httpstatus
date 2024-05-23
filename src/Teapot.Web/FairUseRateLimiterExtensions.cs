using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.RateLimiting;

namespace Teapot.Web;

internal static class FairUseRateLimiterExtensions
{
    internal static string PolicyName = "fair-use";
    public static IServiceCollection AddFairUseRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy(PolicyName, context =>
            {
                string? token = context.Request.Query["token"];
                if (!string.IsNullOrEmpty(token))
                {
                    return RateLimitPartition.GetSlidingWindowLimiter(token,
                        _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            SegmentsPerWindow = 10,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 1000
                        });
                }
                else
                {
                    // people without tokens hit the global limiter
                    return RateLimitPartition.GetConcurrencyLimiter("global",
                        _ => new ConcurrencyLimiterOptions
                        {
                            PermitLimit = 10,
                            QueueLimit = 100,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                }
            });
        });

        return services;
    }

    public static WebApplication UseFairUseRateLimiter(this WebApplication app)
    {
        app.UseRateLimiter();

        return app;
    }
}
