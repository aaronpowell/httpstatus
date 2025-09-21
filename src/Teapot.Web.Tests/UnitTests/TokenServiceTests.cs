using Teapot.Web.Services;

namespace Teapot.Web.Tests.UnitTests;

[TestFixture]
public class TokenServiceTests
{
    [Test]
    public async Task IsValidToken_WithValidToken_ReturnsTrue()
    {
        // Arrange
        var tokenService = new InMemoryTokenService();
        var validToken = Guid.Parse("11111111-1111-1111-1111-111111111111");

        // Act
        var (success, info) = await tokenService.TryGetAuthenticationAsync(validToken);

        // Assert
        Assert.That(success, Is.True);
        Assert.That(info, Is.Not.Null);
    }

    [Test]
    public async Task IsValidToken_WithInvalidToken_ReturnsFalse()
    {
        // Arrange
        var tokenService = new InMemoryTokenService();
        var invalidToken = Guid.NewGuid();

        // Act
        var (success, info) = await tokenService.TryGetAuthenticationAsync(invalidToken);

        // Assert
        Assert.That(success, Is.False);
        Assert.That(info, Is.Null);
    }

    [Test]
    public async Task IsValidToken_WithAllPredefinedTokens_ReturnsTrue()
    {
        // Arrange
        var tokenService = new InMemoryTokenService();
        var predefinedTokens = new[]
        {
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Guid.Parse("33333333-3333-3333-3333-333333333333")
        };

        // Act & Assert
        foreach (var token in predefinedTokens)
        {
            var (success, info) = await tokenService.TryGetAuthenticationAsync(token);
            Assert.That(success, Is.True);
            Assert.That(info, Is.Not.Null);
        }
    }
}