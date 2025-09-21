using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using Teapot.Web.Models;

namespace Teapot.Web;

public class FairUseRateLimiterPolicy(TeapotStatusCodeMetadataCollection statusCodes) : IRateLimiterPolicy<string>
{
    private readonly int[] teapotStatusCodes = [.. statusCodes.Keys.Where(k => k is < 200 or > 299)];

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            // Try read permitted limit from claims
            int permitted = 100;
            var claim = httpContext.User.Claims.FirstOrDefault(c => string.Equals(c.Type, "permitted_limit", StringComparison.OrdinalIgnoreCase));
            if (claim is not null && int.TryParse(claim.Value, out var parsed)) permitted = parsed;

            return RateLimitPartition.GetFixedWindowLimiter(httpContext.User.Identity.Name!,
                partition => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = permitted,
                    QueueLimit = 0,
                    Window = TimeSpan.FromMinutes(1)
                });
        }

        string? clientIp = GetClientIp(httpContext);

        // Use client IP as partition key for anonymous requests when available.
        // Fall back to Host header if IP cannot be determined.
        var partitionKey = clientIp ?? httpContext.Request.Headers.Host.ToString();

        return RateLimitPartition.GetFixedWindowLimiter(partitionKey,
            partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            });
    }

    private static string? GetClientIp(HttpContext context)
    {
        // 1) Try X-Forwarded-For (comma-separated). Use the first parsable IP.
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var xff))
        {
            foreach (var part in xff.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var parsed = TryParseIpCandidate(part);
                if (parsed is not null) return parsed;
            }
        }

        // 2) Try Forwarded header (RFC 7239): look for for=<value> tokens
        if (context.Request.Headers.TryGetValue("Forwarded", out var fwd))
        {
            var fwdVal = fwd.ToString();
            var entries = fwdVal.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var entry in entries)
            {
                var idx = entry.IndexOf("for=", StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    var candidate = entry.Substring(idx + 4).Trim();
                    // strip trailing params separated by ';'
                    var semi = candidate.IndexOf(';');
                    if (semi >= 0) candidate = candidate.Substring(0, semi);
                    var parsed = TryParseIpCandidate(candidate);
                    if (parsed is not null) return parsed;
                }
            }
        }

        // 3) Fallback to connection remote IP (already an IPAddress)
        var remote = context.Connection.RemoteIpAddress;
        if (remote is not null)
        {
            try
            {
                return remote.MapToIPv4().ToString();
            }
            catch
            {
                return remote.ToString();
            }
        }

        return null;
    }

    // Parse a candidate token from headers into a normalized IP string, or null if invalid.
    private static string? TryParseIpCandidate(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        var s = raw.Trim();

        // Remove optional surrounding quotes
        if (s.Length >= 2 && s[0] == '"' && s[^1] == '"') s = s[1..^1].Trim();

        // Trim whitespace again
        s = s.Trim();

        // Handle bracketed IPv6 with port: [2001:db8::1]:1234 -> extract inside brackets
        if (s.StartsWith('['))
        {
            var end = s.IndexOf(']');
            if (end > 1)
            {
                var inner = s.Substring(1, end - 1);
                if (IPAddress.TryParse(inner, out var ip)) return NormalizeIp(ip);
                return null;
            }
        }

        // Handle IPv4:port (1.2.3.4:5678)
        if (s.Count(c => c == ':') == 1 && s.Contains('.'))
        {
            s = s.Split(':')[0];
        }

        // Reject obvious tokens
        if (s.Equals("unknown", StringComparison.OrdinalIgnoreCase)) return null;

        // Try parse, IPAddress.TryParse handles IPv4, IPv6 (and zone IDs on supported runtimes)
        if (IPAddress.TryParse(s, out var parsed))
        {
            return NormalizeIp(parsed);
        }

        return null;
    }

    private static string NormalizeIp(IPAddress ip)
    {
        // Prefer IPv4 representation for IPv4-mapped addresses
        try
        {
            return ip.MapToIPv4().ToString();
        }
        catch
        {
            return ip.ToString();
        }
    }

    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected =>
        (context, _) =>
        {
            var index = Random.Shared.Next(teapotStatusCodes.Length);
            int statusCode = teapotStatusCodes[index];
            context.HttpContext.Response.StatusCode = statusCode;
            return ValueTask.CompletedTask;
        };
}
