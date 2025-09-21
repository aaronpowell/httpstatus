using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Teapot.Web.Services;

/// <summary>
/// In-memory implementation of token service for basic user identity setup.
/// </summary>
public class InMemoryTokenService : ITokenService
{
    private static readonly ConcurrentDictionary<Guid, TokenAuthenticationInfo> _tokenStore = new();

    static InMemoryTokenService()
    {
        _tokenStore[Guid.Parse("11111111-1111-1111-1111-111111111111")] = new TokenAuthenticationInfo(DateTimeOffset.UtcNow.AddDays(-10), "one@example.com", 100);
        _tokenStore[Guid.Parse("22222222-2222-2222-2222-222222222222")] = new TokenAuthenticationInfo(DateTimeOffset.UtcNow.AddDays(-5), "two@example.com", 50);
        _tokenStore[Guid.Parse("33333333-3333-3333-3333-333333333333")] = new TokenAuthenticationInfo(DateTimeOffset.UtcNow.AddDays(-1), "three@example.com", 10);

        // This is the one used by the integration tests and needs a big limit so they all run!
        _tokenStore[Guid.Parse("00000000-0000-0000-0000-000000000000")] = new TokenAuthenticationInfo(DateTimeOffset.UtcNow.AddDays(-30), "zero@example.com", 10_000);
    }

    /// <inheritdoc />
    public Task<(bool Success, TokenAuthenticationInfo? Info)> TryGetAuthenticationAsync(Guid token)
    {
        if (_tokenStore.TryGetValue(token, out var info))
        {
            return Task.FromResult((true, (TokenAuthenticationInfo?)info));
        }

        return Task.FromResult((false, (TokenAuthenticationInfo?)null));
    }
}