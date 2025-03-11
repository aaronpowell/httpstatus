using Microsoft.AspNetCore.Http;
using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;

namespace Teapot.Web.Tests.UnitTests;
public class SuppressBodyTests
{
    private TeapotStatusCodeMetadataCollection _statusCodes;

    [SetUp]
    public void Setup()
    {
        _statusCodes = new(
            new AmazonStatusCodeMetadata(),
            new CloudflareStatusCodeMetadata(),
            new EsriStatusCodeMetadata(),
            new LaravelStatusCodeMetadata(),
            new MicrosoftStatusCodeMetadata(),
            new NginxStatusCodeMetadata(),
            new TwitterStatusCodeMetadata()
        );
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public void SuppressBodyReadFromQuery(bool? suppressBody)
    {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new(), suppressBody: suppressBody), null, request.Object);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.SuppressBody, Is.EqualTo(suppressBody));
        });
    }

    [Test]
    [TestCase("true")]
    [TestCase("false")]
    [TestCase("")]
    public void SuppressBodyReadFromHeader(string? suppressBody)
    {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        request.Object.Headers.Append(StatusExtensions.SUPPRESS_BODY_HEADER, suppressBody);

        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new()), null, request.Object);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            bool? expectedValue = suppressBody switch
            {
                string { Length: > 0 } stringValue => bool.Parse(stringValue),
                _ => null
            };
            Assert.That(r.Options.SuppressBody, Is.EqualTo(expectedValue));
        });
    }

    [Test]
    [TestCase("true", null)]
    [TestCase("false", null)]
    [TestCase("true", false)]
    [TestCase("false", true)]
    [TestCase("", true)]
    [TestCase("", false)]
    public void SuppressBodyReadFromQSTakesPriorityHeader(string? headerValue, bool? queryStringValue)
    {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        request.Object.Headers.Append(StatusExtensions.SUPPRESS_BODY_HEADER, headerValue);

        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new(), suppressBody: queryStringValue), null, request.Object);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            bool? expectedValue = queryStringValue ?? headerValue switch
            {
                string { Length: > 0 } stringValue => bool.Parse(stringValue),
                _ => null
            };
            Assert.That(r.Options.SuppressBody, Is.EqualTo(expectedValue));
        });
    }

    [Test]
    public void BadSuppressBodyHeaderIgnored()
    {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        request.Object.Headers.Append(StatusExtensions.SUPPRESS_BODY_HEADER, "invalid");

        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new()), null, request.Object);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.SuppressBody, Is.Null);
        });
    }
}
