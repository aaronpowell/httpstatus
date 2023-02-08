using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Teapot.Web;

internal class RandomSequenceGenerator : IRandomSequenceGenerator
{
    private readonly Random _random = new();
    internal int[] Range { get; private set; } = Array.Empty<int>();

    public bool TryParse(string range, [NotNullWhen(true)] out IRandomSequenceGenerator? generator)
    {
        try
        {
            generator = new RandomSequenceGenerator
            {
                // copied from https://stackoverflow.com/a/37213725/260221
                Range = range.Split(',')
                                   .Select(x => x.Split('-'))
                                   .Select(p => new { First = int.Parse(p.First()), Last = int.Parse(p.Last()) })
                                   .SelectMany(x => Enumerable.Range(x.First, x.Last - x.First + 1))
                                   .ToArray()
            };
            return true;
        }
        catch
        {
            generator = default;
            return false;
        }
    }

    public int Next => Range[_random.Next(Range.Length)];
}
