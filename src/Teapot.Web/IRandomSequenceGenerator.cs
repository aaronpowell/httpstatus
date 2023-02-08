using System.Diagnostics.CodeAnalysis;

namespace Teapot.Web
{
    public interface IRandomSequenceGenerator
    {
        bool TryParse(string range, [NotNullWhen(true)] out IRandomSequenceGenerator? generator);

        int Next { get; }
    }
}
