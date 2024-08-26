using FluentAssertions;

namespace UnitTests;

public class UnitTest1
{
    [Fact]
    public void TestIfTrueIsTrue()
    {
        true.Should().Be(true);
    }
}