using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Teapot.Web.Middleware;

public class RequireAuthenticatedForTimeoutMiddleware(RequestDelegate next)
{
    private const string SleepQueryParameter = "sleep";
    private const string SleepAfterHeaders = "X-HttpStatus-SleepAfterHeaders";

    public async Task InvokeAsync(HttpContext context)
    {
        // If request contains a timeout query parameter or timeout header and the user is not authenticated,
        // terminate the connection immediately without sending a status code or body.
        bool hasTimeoutQuery = context.Request.Query.ContainsKey(SleepQueryParameter);
        bool hasTimeoutHeader = context.Request.Headers.ContainsKey(SleepAfterHeaders);

        if ((hasTimeoutQuery || hasTimeoutHeader) && context.User?.Identity?.IsAuthenticated != true)
        {
            // Abort() closes the underlying connection immediately. No response will be written.
            context.Abort();
            return;
        }

        await next(context);
    }
}