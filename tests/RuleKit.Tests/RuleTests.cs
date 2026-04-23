namespace RuleKit.Tests;

public class RuleTests
{
    [Fact]
    public void FromPredicate_should_throw_ArgumentNullException_when_predicate_is_null()
    {
        Assert.Throws<ArgumentNullException>(() => Rule.FromPredicate<int>(null!, ""));
    }

    [Fact]
    public void FromPredicate_should_throw_ArgumentNullException_when_message_is_null()
    {
        Assert.Throws<ArgumentNullException>(() => Rule.FromPredicate<int>(_ => true, null!));
    }

    [Fact]
    public void FromPredicate_should_return_valid_result_when_predicate_is_true()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => true, "");

        // act
        var result = rule(1);

        // assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void FromPredicate_should_return_invalid_result_when_predicate_is_false()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => false, "");

        // act
        var result = rule(-1);

        // assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void FromPredicate_should_return_invalid_result_with_message_when_predicate_is_false()
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
