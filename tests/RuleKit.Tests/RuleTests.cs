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
    public void FromPredicate_ShouldReturnPassedResult_WhenPredicateIsTrue()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => true, "failed");

        // act
        var result = rule(1);

        // assert
        Assert.IsType<RulePassed>(result);
    }

    [Fact]
    public void FromPredicate_ShouldReturnFailedResult_WhenPredicateIsFalse()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => false, "failed");

        // act
        var result = rule(-1);

        // assert
        Assert.IsType<RuleFailed>(result);
    }

    [Fact]
    public void FromPredicate_ShouldReturnFailedResultWithMessage_WhenPredicateIsFalse()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => false, "failed");

        // act
        var result = rule(-1);

        // assert
        var failed = Assert.IsType<RuleFailed>(result);
        Assert.Equal("failed", failed.Message);
    }

    [Fact]
    public void Not_ShouldThrowArgumentNullException_WhenRuleIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Rules.Not<int>(null!, "failed"));
        Assert.Equal("rule", exception.ParamName);
    }

    [Fact]
    public void Not_ShouldThrowArgumentNullException_WhenMessageIsNull()
    {
        var rule = Rule.FromPredicate<int>(_ => true, "failed");
        var exception = Assert.Throws<ArgumentNullException>(() => Rules.Not(rule, null!));
        Assert.Equal("message", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Not_ShouldThrowArgumentException_WhenMessageIsEmpty(string message)
    {
        var rule = Rule.FromPredicate<int>(_ => true, "failed");
        var exception = Assert.Throws<ArgumentException>(() => Rules.Not(rule, message));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Not_ShouldReturnFailedResult_WhenInnerRuleReturnsPassedResult()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => true, "failed");
        rule = Rules.Not(rule, "failed");

        // act
        var result = rule(0);

        // assert
        Assert.IsType<RuleFailed>(result);
    }

    [Fact]
    public void Not_ShouldReturnPassedResult_WhenInnerRuleReturnsFailedResult()
    {
        // arrange
        var rule = Rule.FromPredicate<int>(_ => false, "failed");
        rule = Rules.Not(rule, "failed");

        // act
        var result = rule(0);

        // assert
        Assert.IsType<RulePassed>(result);
    }
}
