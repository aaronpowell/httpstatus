using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Teapot.Web.Services;

namespace Teapot.Web.Middleware;

/// <summary>
/// Middleware that handles basic user identity setup based on token authentication.
/// Looks for 'x-httpstatus-token' header or 'token' query string parameter.
/// </summary>
public class UserIdentityMiddleware(RequestDelegate next, ITokenService tokenService)
{
    private const string HeaderName = "x-httpstatus-token";
    private const string QueryParameterName = "token";
    private const string AuthenticationScheme = "TokenAuthentication";

    public async Task InvokeAsync(HttpContext context)
    {
        var token = GetTokenFromRequest(context);
        
        if (token is not null)
        {
            var (success, info) = await tokenService.TryGetAuthenticationAsync(token.Value);
            if (success && info is not null)
            {
                SetUserIdentity(context, token.Value, info);
            }
        }

        await next(context);
    }

    private static Guid? GetTokenFromRequest(HttpContext context)
    {
        // Check header first
        if (context.Request.Headers.TryGetValue(HeaderName, out var headerValue))
        {
            var headerToken = headerValue.ToString();
            if (Guid.TryParse(headerToken, out var parsedHeaderToken))
            {
                return parsedHeaderToken;
            }
        }

        // Check query string parameter
        if (context.Request.Query.TryGetValue(QueryParameterName, out var queryValue))
        {
            var queryToken = queryValue.ToString();
            if (Guid.TryParse(queryToken, out var parsedQueryToken))
            {
                return parsedQueryToken;
            }
        }

        return null;
    }

    private static void SetUserIdentity(HttpContext context, Guid token, TokenAuthenticationInfo info)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, token.ToString()),
            new Claim(ClaimTypes.NameIdentifier, token.ToString()),
            new Claim("token", token.ToString()),
            // Additional claims
            new Claim("token_created", info.Created.ToString("o")),
            new Claim(ClaimTypes.Email, info.ContactEmail ?? string.Empty),
            new Claim("permitted_limit", info.PermittedLimit.ToString())
        };

        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        
        context.User = principal;
    }
}