using System;
using System.Threading.Tasks;

namespace Teapot.Web.Services;

/// <summary>
/// Service for validating user tokens and returning authentication metadata.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Tries to get authentication information for a token.
    /// Returns (true, info) when token is valid; (false, null) when not.
    /// </summary>
    /// <param name="token">The token to validate.</param>
    /// <returns>Tuple of success flag and authentication info (when successful).</returns>
    Task<(bool Success, TokenAuthenticationInfo? Info)> TryGetAuthenticationAsync(Guid token);
}

/// <summary>
/// Additional state returned alongside a successful token lookup.
/// </summary>
public record TokenAuthenticationInfo(DateTimeOffset Created, string? ContactEmail, int PermittedLimit);