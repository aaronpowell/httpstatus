using Microsoft.AspNetCore.Http;
using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;

namespace Teapot.Web.Tests.UnitTests;
public class SleepTests {
    private const int Sleep = 500;

    private TeapotStatusCodeMetadataCollection _statusCodes;

    [SetUp]
    public void Setup() {
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
    public void SleepReadFromQuery()
    {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new(), sleep: Sleep), null, request.Object);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.Sleep, Is.EqualTo(Sleep));
        });
    }

    [Test]
    public void SleepReadFromHeader()
    {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        request.Object.Headers.Append(StatusExtensions.SLEEP_HEADER, Sleep.ToString());

        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new()), null, request.Object);

        Assert.Multiple(() => {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.Sleep, Is.EqualTo(Sleep));
        });
    }

    [Test]
    public void SleepReadFromQSTakesPriorityHeader() {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        request.Object.Headers.Append(StatusExtensions.SLEEP_HEADER, Sleep.ToString());

        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new(), sleep: Sleep * 2), null, request.Object);

        Assert.Multiple(() => {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.Sleep, Is.EqualTo(Sleep * 2));
        });
    }

    [Test]
    public void BadSleepHeaderIgnored()
    {
        Mock<HttpRequest> request = HttpRequestHelper.GenerateMockRequest();
        request.Object.Headers.Append(StatusExtensions.SLEEP_HEADER, "invalid");

        IResult result = StatusExtensions.CommonHandleStatusRequestAsync(new ResponseOptions(200, new()), null, request.Object);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Options.Sleep, Is.Null);
        });
    }

}
