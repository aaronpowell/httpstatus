using Teapot.Web.Models;
using Teapot.Web.Models.Unofficial;
using static System.Net.HttpStatusCode;

namespace Teapot.Web.Tests;

public class TestCases
{
    private static readonly TeapotStatusCodeResults All = new(
            new AmazonStatusCodeResults(),
            new CloudflareStatusCodeResults(),
            new EsriStatusCodeResults(),
            new LaravelStatusCodeResults(),
            new MicrosoftStatusCodeResults(),
            new NginxStatusCodeResults(),
            new TwitterStatusCodeResults()
            );

    private static readonly HttpStatusCode[] NoContentStatusCodes = new[]
    {
        Continue, SwitchingProtocols, Processing, EarlyHints, NoContent, ResetContent, NotModified
    };

    public static IEnumerable<TestCaseCodes> StatusCodesAll =>
        All.Select(Map);

    public static IEnumerable<TestCaseCodes> StatusCodesWithContent =>
        All
        .Where(x => !x.Value.ExcludeBody)
        .Select(Map);

    public static IEnumerable<TestCaseCodes> StatusCodesNoContent =>
        NoContentStatusCodes.Select(Map);

    private static TestCaseCodes Map(HttpStatusCode code)
    {
        var key = (int)code;
        return new(key, All[key].Description, All[key].Body);
    }

    private static TestCaseCodes Map(KeyValuePair<int, TeapotStatusCodeResult> code)
    {
        return new TestCaseCodes(code.Key, code.Value.Description, code.Value.Body);
    }
}
