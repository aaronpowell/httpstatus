namespace Teapot.Web.Tests.UnitTests;

public class RandomSequenceGeneratorTests
{
    private RandomSequenceGenerator _target;

    [SetUp]
    public void Setup()
    {
        _target = new RandomSequenceGenerator();
    }

    [TestCase("foo")]
    [TestCase("200,x")]
    [TestCase("200.0")]
    [TestCase("-1,0,1")]
    [TestCase("-1-1")]
    public void ParseError(string input)
    {
        var result = _target.TryParse(input, out var _);

        Assert.That(result, Is.False);
    }

    [TestCase("1,2,3", 1, 2, 3)]
    [TestCase("1-3", 1, 2, 3)]
    [TestCase("1,2,3,4-6", 1, 2, 3, 4, 5, 6)]
    [TestCase("1, 2, 3 , 4 - 6", 1, 2, 3, 4, 5, 6)]
    [TestCase("1,2,1,2", 1, 2, 1, 2)]
    [TestCase("1-3,2-5", 1, 2, 3, 2, 3, 4, 5)]
    public void ParsedAsExpected(string input, params int[] expected)
    {
        var result = _target.TryParse(input, out var random);

        Assert.That(result, Is.True);
        Assert.That(random, Is.Not.Null);
        var randomInstance = random as RandomSequenceGenerator;
        Assert.That(randomInstance, Is.Not.Null);
        Assert.That(randomInstance.Range, Is.EqualTo(expected));
    }
}
