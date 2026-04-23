namespace RuleKit.Tests;

public class RuleTests
{
    [Fact]
    public void FromPredicate_ShouldThrowArgumentNullException_WhenPredicateIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Rule.FromPredicate<int>(null!, ""));
    }

    [Fact]
    public void FromPredicate_ShouldThrowArgumentNullException_WhenMessageIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Rule.FromPredicate<int>(_ => true, null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void FromPredicate_ShouldThrowArgumentException_WhenMessageIsEmpty(string message)
    {
        Assert.Throws<ArgumentException>(() => Rule.FromPredicate<int>(_ => true, message));
    }

    [Fact]
    public void FromPredicate_ShouldReturnValidResult_WhenPredicateIsTrue()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => true, "failed");

        // act
        var result = rule(1);

        // assert
        Assert.IsType<ValidRuleResult>(result);
    }

    [Fact]
    public void FromPredicate_ShouldReturnInvalidResult_WhenPredicateIsFalse()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => false, "failed");

        // act
        var result = rule(-1);

        // assert
        Assert.IsType<InvalidRuleResult>(result);
    }

    [Fact]
    public void FromPredicate_ShouldReturnInvalidResultWithMessage_WhenPredicateIsFalse()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(x => x > 0, "failed");

        // act
        var result = rule(-1);

        // assert
        var invalid = Assert.IsType<InvalidRuleResult>(result);
        Assert.Equal("failed", invalid.Message);
    }
}
