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

    [Fact]
    public void FromPredicate_ShouldReturnValidResult_WhenPredicateIsTrue()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => true, "");

        // act
        var result = rule(1);

        // assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void FromPredicate_ShouldReturnInvalidResult_WhenPredicateIsFalse()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => false, "");

        // act
        var result = rule(-1);

        // assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void FromPredicate_ShouldReturnInvalidResultWithMessage_WhenPredicateIsFalse()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(x => x > 0, "failed");

        // act
        var result = rule(-1);

        // assert
        Assert.False(result.IsValid);
        Assert.Equal("failed", result.Message);
    }
}
