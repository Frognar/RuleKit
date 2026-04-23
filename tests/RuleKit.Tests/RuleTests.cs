namespace RuleKit.Tests;

public class RuleTests
{
    [Fact]
    public void FromPredicate_ShouldThrowArgumentNullException_WhenPredicateIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Rule.FromPredicate<int>(null!, ""));
        Assert.Equal("predicate", exception.ParamName);
    }

    [Fact]
    public void FromPredicate_ShouldThrowArgumentNullException_WhenMessageIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Rule.FromPredicate<int>(_ => true, null!));
        Assert.Equal("message", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void FromPredicate_ShouldThrowArgumentException_WhenMessageIsEmpty(string message)
    {
        var exception = Assert.Throws<ArgumentException>(() => Rule.FromPredicate<int>(_ => true, message));
        Assert.Equal("message", exception.ParamName);
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
