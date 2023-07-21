using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teapot.Web.Controllers;
using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;

namespace Teapot.Web.Tests.UnitTests;
public class SuppressBodyTests {
    private TeapotStatusCodeResults _statusCodes;

    [SetUp]
    public void Setup() {
        _statusCodes = new(
            new AmazonStatusCodeResults(),
            new CloudflareStatusCodeResults(),
            new EsriStatusCodeResults(),
            new LaravelStatusCodeResults(),
            new MicrosoftStatusCodeResults(),
            new NginxStatusCodeResults(),
            new TwitterStatusCodeResults()
        );
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public void SuppressBodyReadFromQuery(bool? suppressBody) {
        StatusController controller = new(_statusCodes)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        IActionResult result = controller.StatusCode(200, null, suppressBody);

        Assert.Multiple(() => {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.SuppressBody, Is.EqualTo(suppressBody));
        });
    }

    [Test]
    [TestCase("true")]
    [TestCase("false")]
    [TestCase("")]
    public void SuppressBodyReadFromHeader(string? suppressBody)
    {
        StatusController controller = new(_statusCodes)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.ControllerContext.HttpContext.Request.Headers.Add(StatusController.SUPPRESS_BODY_HEADER, suppressBody);

        IActionResult result = controller.StatusCode(200, null, null);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            var expectedValue = suppressBody switch
            {
                string { Length: >0 } stringValue => (bool?)bool.Parse(stringValue),
                _ => null
            };
            Assert.That(r.SuppressBody, Is.EqualTo(expectedValue));
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
        StatusController controller = new(_statusCodes)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.ControllerContext.HttpContext.Request.Headers.Add(StatusController.SUPPRESS_BODY_HEADER, headerValue);

        IActionResult result = controller.StatusCode(200, null, queryStringValue);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());
            
            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            var expectedValue = queryStringValue.HasValue ? queryStringValue.Value : headerValue switch
            {
                string { Length: > 0 } stringValue => (bool?)bool.Parse(stringValue),
                _ => null
            };
            Assert.That(r.SuppressBody, Is.EqualTo(expectedValue));
        });
    }

    [Test]
    public void BadSuppressBodyHeaderIgnored()
    {
        StatusController controller = new(_statusCodes)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.ControllerContext.HttpContext.Request.Headers.Add(StatusController.SUPPRESS_BODY_HEADER, "invalid");

        IActionResult result = controller.StatusCode(200, null, null);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.SuppressBody, Is.Null);
        });
    }
}
