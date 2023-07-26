using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Teapot.Web.Models;

namespace Teapot.Web.Tests.UnitTests;

public class CustomHttpStatusCodeResultTests
{
    private HttpContext _httpContext;
    private HttpResponseFeature _httpResponseFeature;
    private Stream _body;

    [SetUp]
    public void Setup()
    {
        Mock<ILogger> logger = new();
        Mock<ILoggerFactory> loggerFactory = new();
        Mock<IServiceProvider> serviceProvider = new();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
        serviceProvider.Setup(x => x.GetService(typeof(ILogger))).Returns(logger.Object);
        serviceProvider.Setup(x => x.GetService(typeof(ILoggerFactory))).Returns(loggerFactory.Object);

        FeatureCollection featureCollection = new();
        _httpResponseFeature = new HttpResponseFeature();
        _body = new MemoryStream();
        featureCollection.Set<IHttpRequestFeature>(new HttpRequestFeature());
        featureCollection.Set<IHttpResponseFeature>(_httpResponseFeature);
        featureCollection.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(_body));

        _httpContext = new DefaultHttpContext(featureCollection)
        {
            RequestServices = serviceProvider.Object
        };
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesAll))]
    public async Task Response_Is_Correct(TestCase testCase) {
        TeapotStatusCodeMetadata statusCodeResult = new()
        {
            Description = testCase.Description
        };

        CustomHttpStatusCodeResult target = new(testCase.Code, statusCodeResult, null, null, new());

        await target.ExecuteAsync(_httpContext);
        Assert.Multiple(() => {
            Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(testCase.Code));
            Assert.That(_httpContext.Response.ContentType, Is.EqualTo("text/plain"));
            Assert.That(_httpResponseFeature.ReasonPhrase, Is.EqualTo(testCase.Description));
        });
        _body.Position = 0;
        StreamReader sr = new(_body);
        string body = sr.ReadToEnd();
        Assert.Multiple(() => {
            Assert.That(body, Is.EqualTo(testCase.ToString()));
            Assert.That(_httpContext.Response.ContentLength, Is.EqualTo(body.Length));
        });
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesAll))]
    public async Task Response_Json_Is_Correct(TestCase testCase) {
        TeapotStatusCodeMetadata statusCodeResult = new()
        {
            Description = testCase.Description
        };

        CustomHttpStatusCodeResult target = new(testCase.Code, statusCodeResult, null, null, new());

        _httpContext.Request.Headers.Accept = "application/json";

        await target.ExecuteAsync(_httpContext);
        Assert.Multiple(() => {
            Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(testCase.Code));
            Assert.That(_httpContext.Response.ContentType, Is.EqualTo("application/json"));
            Assert.That(_httpResponseFeature.ReasonPhrase, Is.EqualTo(testCase.Description));
        });
        _body.Position = 0;
        StreamReader sr = new(_body);
        string body = sr.ReadToEnd();
        string expectedBody = JsonSerializer.Serialize(testCase);
        Assert.Multiple(() => {
            Assert.That(body, Is.EqualTo(expectedBody));
            Assert.That(_httpContext.Response.ContentLength, Is.EqualTo(body.Length));
        });
    }

    [TestCaseSource(typeof(TestCases), nameof(TestCases.StatusCodesNoContent))]
    public async Task Response_No_Content(TestCase testCase) {
        TeapotStatusCodeMetadata statusCodeResult = new()
        {
            Description = testCase.Description,
            ExcludeBody = true
        };

        _httpContext.Response.Headers["Content-Type"] = "text/plain";
        _httpContext.Response.Headers["Content-Length"] = "42";

        CustomHttpStatusCodeResult target = new(testCase.Code, statusCodeResult, null, null, new());

        await target.ExecuteAsync(_httpContext);
        Assert.Multiple(() => {
            Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(testCase.Code));
            Assert.That(_httpContext.Response.ContentType, Is.Null);
            Assert.That(_httpResponseFeature.ReasonPhrase, Is.EqualTo(testCase.Description));
        });
        _body.Position = 0;
        StreamReader sr = new(_body);
        string body = sr.ReadToEnd();
        Assert.Multiple(() => {
            Assert.That(body, Is.Empty);
            Assert.That(_httpContext.Response.ContentLength, Is.Null);
        });
    }
}