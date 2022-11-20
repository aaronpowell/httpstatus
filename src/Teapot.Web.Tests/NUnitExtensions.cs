using NUnit.Framework.Constraints;
using System.Text.RegularExpressions;

namespace Teapot.Web.Tests;

public static class NUnitExtensions
{
    private static readonly Regex _nonAlphanumericRegex = new("[^a-zA-Z0-9]+");

    private static string ReplaceNonAlphanumeric(string s) => _nonAlphanumericRegex.Replace(s, "").ToLowerInvariant();

    public static EqualConstraint OnlyAlphanumericIgnoreCase(this EqualConstraint constraint)
        => constraint.Using<string>((actual, expected) => ReplaceNonAlphanumeric(actual) == ReplaceNonAlphanumeric(expected));
}