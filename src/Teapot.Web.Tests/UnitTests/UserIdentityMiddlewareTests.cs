using Microsoft.AspNetCore.Http;
using Teapot.Web.Middleware;
using Teapot.Web.Services;

namespace Teapot.Web.Tests.UnitTests;

[TestFixture]
public class UserIdentityMiddlewareTests
{
    [Test]
    public async Task InvokeAsync_WithValidTokenInHeader_SetsUserIdentity()
    {
        // Arrange
        var validToken = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var mockTokenService = new Mock<ITokenService>();
        mockTokenService.Setup(x => x.TryGetAuthenticationAsync(validToken))
            .Returns(Task.FromResult<(bool, TokenAuthenticationInfo?)>((true, new TokenAuthenticationInfo(DateTimeOffset.UtcNow, "one@example.com", 100))));

        var middleware = new UserIdentityMiddleware(_ => Task.CompletedTask, mockTokenService.Object);
        
        var context = new DefaultHttpContext();
        context.Request.Headers["x-httpstatus-token"] = validToken.ToString();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.That(context.User.Identity?.IsAuthenticated, Is.True);
        Assert.That(context.User.Identity.Name, Is.EqualTo(validToken.ToString()));
        Assert.That(context.User.Identity.AuthenticationType, Is.EqualTo("TokenAuthentication"));
    }

    [Test]
    public async Task InvokeAsync_WithValidTokenInQueryString_SetsUserIdentity()
    {
        // Arrange
        var validToken = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var mockTokenService = new Mock<ITokenService>();
        mockTokenService.Setup(x => x.TryGetAuthenticationAsync(validToken))
            .Returns(Task.FromResult<(bool, TokenAuthenticationInfo?)>((true, new TokenAuthenticationInfo(DateTimeOffset.UtcNow, "two@example.com", 50))));

        var middleware = new UserIdentityMiddleware(_ => Task.CompletedTask, mockTokenService.Object);
        
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString($"?token={validToken}");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.That(context.User.Identity?.IsAuthenticated, Is.True);
        Assert.That(context.User.Identity.Name, Is.EqualTo(validToken.ToString()));
        Assert.That(context.User.Identity.AuthenticationType, Is.EqualTo("TokenAuthentication"));
    }

    [Test]
    public async Task InvokeAsync_WithInvalidToken_DoesNotSetUserIdentity()
    {
        // Arrange
        var invalidToken = Guid.NewGuid();
        var mockTokenService = new Mock<ITokenService>();
        mockTokenService.Setup(x => x.TryGetAuthenticationAsync(It.IsAny<Guid>())).Returns(Task.FromResult((false, (TokenAuthenticationInfo?)null)));

        var middleware = new UserIdentityMiddleware(_ => Task.CompletedTask, mockTokenService.Object);
        
        var context = new DefaultHttpContext();
        context.Request.Headers["x-httpstatus-token"] = invalidToken.ToString();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.That(context.User.Identity?.IsAuthenticated, Is.False);
        Assert.That(context.User.Identity.Name, Is.Null);
    }

    [Test]
    public async Task InvokeAsync_WithMalformedToken_DoesNotSetUserIdentity()
    {
        // Arrange
        var mockTokenService = new Mock<ITokenService>();
        var middleware = new UserIdentityMiddleware(_ => Task.CompletedTask, mockTokenService.Object);
        
        var context = new DefaultHttpContext();
        context.Request.Headers["x-httpstatus-token"] = "not-a-guid";

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.That(context.User.Identity?.IsAuthenticated, Is.False);
        Assert.That(context.User.Identity.Name, Is.Null);
        mockTokenService.Verify(x => x.TryGetAuthenticationAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public async Task InvokeAsync_WithNoToken_DoesNotSetUserIdentity()
    {
        // Arrange
        var mockTokenService = new Mock<ITokenService>();
        var middleware = new UserIdentityMiddleware(_ => Task.CompletedTask, mockTokenService.Object);
        
        var context = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.That(context.User.Identity?.IsAuthenticated, Is.False);
        Assert.That(context.User.Identity.Name, Is.Null);
        mockTokenService.Verify(x => x.TryGetAuthenticationAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public async Task InvokeAsync_HeaderTakesPrecedenceOverQueryString()
    {
        // Arrange
        var headerToken = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var queryToken = Guid.Parse("22222222-2222-2222-2222-222222222222");
        
        var mockTokenService = new Mock<ITokenService>();
        mockTokenService.Setup(x => x.TryGetAuthenticationAsync(headerToken)).Returns(Task.FromResult<(bool, TokenAuthenticationInfo?)>((true, new TokenAuthenticationInfo(DateTimeOffset.UtcNow, "one@example.com", 100))));
        mockTokenService.Setup(x => x.TryGetAuthenticationAsync(queryToken)).Returns(Task.FromResult<(bool, TokenAuthenticationInfo?)>((true, new TokenAuthenticationInfo(DateTimeOffset.UtcNow, "two@example.com", 50))));

        var middleware = new UserIdentityMiddleware(_ => Task.CompletedTask, mockTokenService.Object);
        
        var context = new DefaultHttpContext();
        context.Request.Headers["x-httpstatus-token"] = headerToken.ToString();
        context.Request.QueryString = new QueryString($"?token={queryToken}");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.That(context.User.Identity?.IsAuthenticated, Is.True);
        Assert.That(context.User.Identity.Name, Is.EqualTo(headerToken.ToString()));
        mockTokenService.Verify(x => x.TryGetAuthenticationAsync(headerToken), Times.Once);
        mockTokenService.Verify(x => x.TryGetAuthenticationAsync(queryToken), Times.Never);
    }
}