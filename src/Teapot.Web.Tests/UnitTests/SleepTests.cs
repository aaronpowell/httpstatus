using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teapot.Web.Controllers;
using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;

namespace Teapot.Web.Tests.UnitTests;
internal class SleepTests {
    private const int Sleep = 500;

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
    public void SleepReadFromQuery() {
        StatusController controller = new(_statusCodes);

        IActionResult result = controller.StatusCode(200, Sleep);

        Assert.Multiple(() => {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Sleep, Is.EqualTo(Sleep));
        });
    }

    [Test]
    public void SleepReadFromHeader() {
        StatusController controller = new(_statusCodes) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.ControllerContext.HttpContext.Request.Headers.Add(StatusController.SLEEP_HEADER, Sleep.ToString());

        IActionResult result = controller.StatusCode(200, null);

        Assert.Multiple(() => {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Sleep, Is.EqualTo(Sleep));
        });
    }

    [Test]
    public void SleepReadFromQSTakesPriorityHeader() {
        StatusController controller = new(_statusCodes) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.ControllerContext.HttpContext.Request.Headers.Add(StatusController.SLEEP_HEADER, Sleep.ToString());

        IActionResult result = controller.StatusCode(200, Sleep * 2);

        Assert.Multiple(() => {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Sleep, Is.EqualTo(Sleep * 2));
        });
    }

    [Test]
    public void BadSleepHeaderIgnored() {
        StatusController controller = new(_statusCodes) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.ControllerContext.HttpContext.Request.Headers.Add(StatusController.SLEEP_HEADER, "invalid");

        IActionResult result = controller.StatusCode(200, null);

        Assert.Multiple(() => {
            Assert.That(result, Is.InstanceOf<CustomHttpStatusCodeResult>());

            CustomHttpStatusCodeResult r = (CustomHttpStatusCodeResult)result;
            Assert.That(r.Sleep, Is.Null);
        });
    }
}
