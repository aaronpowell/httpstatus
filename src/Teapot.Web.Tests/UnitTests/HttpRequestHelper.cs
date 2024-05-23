using Microsoft.AspNetCore.Http;

namespace Teapot.Web.Tests.UnitTests;

internal static class HttpRequestHelper
{
    public static Mock<HttpRequest> GenerateMockRequest()
    {
        Mock<HttpRequest> request = new();
        request.Setup(Setup => Setup.Headers).Returns(new HeaderDictionary());
        return request;
    }
}
