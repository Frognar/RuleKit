namespace RuleKit.Tests;

public class RuleTests
{
    private readonly Rule<int> alwaysPassingRule = Rule.FromPredicate<int>(_ => true, "failed");
    private readonly Rule<int> alwaysFailingRule = Rule.FromPredicate<int>(_ => false, "failed");
    private static Rule<T> CreateRule<T>(Func<T, bool> predicate, string msg) => Rule.FromPredicate(predicate, msg);
    private static Rule<T> Negate<T>(Rule<T> rule, string msg) => Rules.Not(rule, msg);

    [Fact]
    public void FromPredicate_ShouldThrowArgumentNullException_WhenPredicateIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => CreateRule<int>(null!, ""));
        Assert.Equal("predicate", exception.ParamName);
    }

    [Fact]
    public void FromPredicate_ShouldThrowArgumentNullException_WhenMessageIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => CreateRule<int>(_ => true, null!));
        Assert.Equal("message", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void FromPredicate_ShouldThrowArgumentException_WhenMessageIsEmpty(string message)
    {
        var exception = Assert.Throws<ArgumentException>(() => CreateRule<int>(_ => true, message));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void FromPredicate_ShouldReturnPassedResult_WhenPredicateIsTrue()
    {
        // act
        var result = alwaysPassingRule(0);

        // assert
        Assert.IsType<RulePassed>(result);
    }

    [Fact]
    public void FromPredicate_ShouldReturnFailedResult_WhenPredicateIsFalse()
    {
        // act
        var result = alwaysFailingRule(0);

        // assert
        Assert.IsType<RuleFailed>(result);
    }

    [Fact]
    public void FromPredicate_ShouldReturnFailedResultWithMessage_WhenPredicateIsFalse()
    {
        // act
        var result = alwaysFailingRule(0);

        // assert
        var failed = Assert.IsType<RuleFailed>(result);
        Assert.Equal("failed", failed.Message);
    }

    [Fact]
    public void Not_ShouldThrowArgumentNullException_WhenRuleIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Negate<int>(null!, "failed"));
        Assert.Equal("rule", exception.ParamName);
    }

    [Fact]
    public void Not_ShouldThrowArgumentNullException_WhenMessageIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Negate(alwaysPassingRule, null!));
        Assert.Equal("message", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Not_ShouldThrowArgumentException_WhenMessageIsEmpty(string message)
    {
        var exception = Assert.Throws<ArgumentException>(() => Negate(alwaysPassingRule, message));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Not_ShouldReturnFailedResult_WhenInnerRuleReturnsPassedResult()
    {
        // arrange
        var rule = Negate(alwaysPassingRule, "failed");

        // act
        var result = rule(0);

        // assert
        Assert.IsType<RuleFailed>(result);
    }

    [Fact]
    public void Not_ShouldReturnPassedResult_WhenInnerRuleReturnsFailedResult()
    {
        // arrange
        var rule = Negate(alwaysFailingRule, "failed");

        // act
        var result = rule(0);

        // assert
        Assert.IsType<RulePassed>(result);
    }
}
