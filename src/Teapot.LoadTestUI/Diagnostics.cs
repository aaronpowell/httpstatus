using System.Diagnostics;

namespace Teapot.LoadTestUI;

internal static class Diagnostics
{
    public static readonly ActivitySource ActivitySource = new("Teapot.LoadTestUI", "1.0.0");
}