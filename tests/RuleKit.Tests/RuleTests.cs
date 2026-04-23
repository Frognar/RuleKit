namespace RuleKit.Tests;

public class RuleTests
{
    [Fact]
    void FromPredicate_should_throw_ArgumentNullException_when_predicate_is_null()
    {
        Assert.Throws<ArgumentNullException>(() => Rule.FromPredicate<int>(null!));
    }

    [Fact]
    void FromPredicate_should_return_valid_result_when_predicate_is_true()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(x => x > 0);

        // act
        var result = rule(1);

        // assert
        Assert.True(result.IsValid);
    }
}
