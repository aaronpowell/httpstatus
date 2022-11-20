using static System.Net.HttpStatusCode;

namespace Teapot.Web.Tests;

public class ExtendedHttpStatusCodes
{
    private static readonly ExtendedHttpStatusCode[] NonStandardStatusCodes = new[] {
            new ExtendedHttpStatusCode(300, "Multiple Choices"),
            new ExtendedHttpStatusCode(301, "Moved Permanently"),
            new ExtendedHttpStatusCode(302, "Found"),
            new ExtendedHttpStatusCode(303, "See Other"),
            new ExtendedHttpStatusCode(306, "Switch Proxy"),
            new ExtendedHttpStatusCode(418, "I'm a teapot"),
            new ExtendedHttpStatusCode(419, "CSRF Token Missong or Expired"),
            new ExtendedHttpStatusCode(420, "Enhance Your Calm"),
            new ExtendedHttpStatusCode(425, "Too Early"),
            new ExtendedHttpStatusCode(440, "Login Time-out"),
            new ExtendedHttpStatusCode(444, "No Response"),
            new ExtendedHttpStatusCode(449, "Retry With"),
            new ExtendedHttpStatusCode(450, "Blocked by Windows Parental Controls"),
            new ExtendedHttpStatusCode(460, "Client closed the connection with AWS Elastic Load Balancer"),
            new ExtendedHttpStatusCode(463, "The load balancer received an X-Forwarded-For request header with more than 30 IP addresses"),
            new ExtendedHttpStatusCode(494, "Request header too large"),
            new ExtendedHttpStatusCode(495, "SSL Certificate Error"),
            new ExtendedHttpStatusCode(496, "SSL Certificate Required"),
            new ExtendedHttpStatusCode(497, "HTTP Request Sent to HTTPS Port"),
            new ExtendedHttpStatusCode(498, "Invalid Token (Esri)"),
            new ExtendedHttpStatusCode(499, "Client Closed Request"),
            new ExtendedHttpStatusCode(520, "Web Server Returned an Unknown Error"),
            new ExtendedHttpStatusCode(521, "Web Server Is Down"),
            new ExtendedHttpStatusCode(522, "Connection Timed out"),
            new ExtendedHttpStatusCode(523, "Origin Is Unreachable"),
            new ExtendedHttpStatusCode(524, "A Timeout Occurred"),
            new ExtendedHttpStatusCode(525, "SSL Handshake Failed"),
            new ExtendedHttpStatusCode(526, "Invalid SSL Certificate"),
            new ExtendedHttpStatusCode(527, "Railgun Error"),
            new ExtendedHttpStatusCode(530, "Origin DNS Error"),
            new ExtendedHttpStatusCode(561, "Unauthorized (AWS Elastic Load Balancer)")
        };

    private static readonly HttpStatusCode[] OfficialStatusCodes = Enum.GetValues<HttpStatusCode>();

    private static readonly HttpStatusCode[] NoContentStatusCodes = new[]
    {
        SwitchingProtocols, NoContent, ResetContent, NotModified
    };

    private static readonly HttpStatusCode[] DifferentContentStatusCodes = new[]
    {
        MultiStatus
    };

    private static readonly HttpStatusCode[] ServerErrorStatusCodes = new[]
    {
        Continue, Processing, EarlyHints
    };

    public static IEnumerable<ExtendedHttpStatusCode> StatusCodesAll =>
        NonStandardStatusCodes
        .Union(OfficialStatusCodes.Select(Map));

    public static IEnumerable<ExtendedHttpStatusCode> StatusCodesWithContent =>
        NonStandardStatusCodes
        .Union(OfficialStatusCodes
                   .Except(NoContentStatusCodes)
                   .Except(ServerErrorStatusCodes)
                   .Except(DifferentContentStatusCodes)
                   .Select(Map));

    public static IEnumerable<ExtendedHttpStatusCode> StatusCodesNoContent =>
        NoContentStatusCodes.Select(Map);

    public static IEnumerable<ExtendedHttpStatusCode> StatusCodesServerError =>
        ServerErrorStatusCodes.Select(Map);

    private static ExtendedHttpStatusCode Map(HttpStatusCode httpStatusCode)
        => new((int)httpStatusCode, httpStatusCode.ToString());
}
