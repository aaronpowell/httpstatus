using static System.Net.Http.HttpMethod;

namespace Teapot.Web.Tests;

public class HttpMethods
{
    public static HttpMethod[] All => new[] { Get, Put, Post, Delete, Head, Options, Trace, Patch };
}
