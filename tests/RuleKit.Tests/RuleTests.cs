namespace RuleKit.Tests;

public class RuleTests
{
    private readonly Func<int, bool> alwaysTruePredicate = _ => true;
    private readonly Func<int, bool> alwaysFalsePredicate = _ => false;
    private static Rule<T> CreateRule<T>(Func<T, bool> predicate, string code = "general", string message = "failed") =>
        Rule.FromPredicate(predicate, code, message);

    private static Rule<T> Negate<T>(Rule<T> rule, string code = "general", string message = "failed") =>
        Rules.Not(rule, code, message);
    
    private static Rule<T> And<T>(Rule<T> left, Rule<T> right) =>
        Rules.And(left, right);

    private static Rule<T> Or<T>(Rule<T> left, Rule<T> right) =>
        Rules.Or(left, right);

    [Fact]
    public void FromPredicate_ShouldThrowArgumentNullException_WhenPredicateIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => CreateRule<int>(null!));
        Assert.Equal("predicate", exception.ParamName);
    }

    [Fact]
    public void FromPredicate_ShouldThrowArgumentNullException_WhenCodeIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => CreateRule(alwaysTruePredicate, code: null!));
        Assert.Equal("code", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void FromPredicate_ShouldThrowArgumentException_WhenCodeIsEmpty(string code)
    {
        var exception = Assert.Throws<ArgumentException>(() => CreateRule(alwaysTruePredicate, code: code));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void FromPredicate_ShouldThrowArgumentNullException_WhenMessageIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => CreateRule(alwaysTruePredicate, message: null!));
        Assert.Equal("message", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void FromPredicate_ShouldThrowArgumentException_WhenMessageIsEmpty(string message)
    {
        var exception = Assert.Throws<ArgumentException>(() => CreateRule(alwaysTruePredicate, message: message));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void FromPredicate_ShouldReturnPassedResult_WhenPredicateIsTrue()
    {
        // arrange
        var rule = CreateRule(alwaysTruePredicate);

        // act
        var result = rule(0);

        // assert
        Assert.IsType<RulePassed>(result);
    }

    [Fact]
    public void FromPredicate_ShouldReturnFailedResult_WhenPredicateIsFalse()
    {
        // arrange
        var rule = CreateRule(alwaysFalsePredicate);

        // act
        var result = rule(0);

        // assert
        Assert.IsType<RuleFailed>(result);
    }

    [Fact]
    public void FromPredicate_ShouldReturnFailedResultWithCode_WhenPredicateIsFalse()
    {
        // arrange
        var rule = CreateRule(alwaysFalsePredicate, code: "always-false");

        // act
        var result = rule(0);

        // assert
        var failed = Assert.IsType<RuleFailed>(result);
        Assert.Equal("always-false", failed.Code);
    }

    [Fact]
    public void FromPredicate_ShouldReturnFailedResultWithMessage_WhenPredicateIsFalse()
    {
        // arrange
        var rule = CreateRule(alwaysFalsePredicate, message: "always-false");

        // act
        var result = rule(0);

        // assert
        var failed = Assert.IsType<RuleFailed>(result);
        Assert.Equal("always-false", failed.Message);
    }

    [Fact]
    public void Not_ShouldThrowArgumentNullException_WhenRuleIsNull()
    {
        // act
        var exception = Assert.Throws<ArgumentNullException>(() => Negate<int>(null!));

        // assert
        Assert.Equal("rule", exception.ParamName);
    }

    [Fact]
    public void Not_ShouldThrowArgumentNullException_WhenCodeIsNull()
    {
        // arrange
        var rule = CreateRule(alwaysTruePredicate);
        
        // act
        var exception = Assert.Throws<ArgumentNullException>(() => Negate(rule, code: null!));

        // assert
        Assert.Equal("code", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Not_ShouldThrowArgumentException_WhenCodeIsEmpty(string code)
    {
        // arrange
        var rule = CreateRule(alwaysTruePredicate);

        // act
        var exception = Assert.Throws<ArgumentException>(() => Negate(rule, code: code));

        // assert
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void Not_ShouldThrowArgumentNullException_WhenMessageIsNull()
    {
        // arrange
        var rule = CreateRule(alwaysTruePredicate);

        // act
        var exception = Assert.Throws<ArgumentNullException>(() => Negate(rule, message: null!));

        // assert
        Assert.Equal("message", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Not_ShouldThrowArgumentException_WhenMessageIsEmpty(string message)
    {
        // arrange
        var rule = CreateRule(alwaysTruePredicate);

        // act
        var exception = Assert.Throws<ArgumentException>(() => Negate(rule, message: message));

        // assert
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Not_ShouldReturnFailedResult_WhenInnerRuleReturnsPassedResult()
    {
        // arrange
        var rule = CreateRule(alwaysTruePredicate);
        rule = Negate(rule);

        // act
        var result = rule(0);

        // assert
        Assert.IsType<RuleFailed>(result);
    }

    [Fact]
    public void Not_ShouldReturnFailedResultWithCode_WhenInnerRuleReturnsPassedResult()
    {
        // arrange
        var rule = CreateRule(alwaysTruePredicate);
        rule = Negate(rule, code: "negation");

        // act
        var result = rule(0);

        // assert
        var failed = Assert.IsType<RuleFailed>(result);
        Assert.Equal("negation", failed.Code);
    }

    [Fact]
    public void Not_ShouldReturnFailedResultWithMessage_WhenInnerRuleReturnsPassedResult()
    {
        // arrange
        var rule = CreateRule(alwaysTruePredicate);
        rule = Negate(rule, message: "negation");

        // act
        var result = rule(0);

        // assert
        var failed = Assert.IsType<RuleFailed>(result);
        Assert.Equal("negation", failed.Message);
    }

    [Fact]
    public void Not_ShouldReturnPassedResult_WhenInnerRuleReturnsFailedResult()
    {
        // arrange
        var rule = CreateRule(alwaysFalsePredicate);
        rule = Negate(rule);

        // act
        var result = rule(0);

        // assert
        Assert.IsType<RulePassed>(result);
    }

    [Fact]
    public void And_ShouldThrowArgumentNullException_WhenRightIsNull()
    {
        // arrange
        var left = CreateRule(alwaysTruePredicate);

        // act
        var exception = Assert.Throws<ArgumentNullException>(() => And(left, null!));

        // assert
        Assert.Equal("right", exception.ParamName);
    }

    [Fact]
    public void And_ShouldThrowArgumentNullException_WhenLeftIsNull()
    {
        // arrange
        var right = CreateRule(alwaysTruePredicate);

        // act
        var exception = Assert.Throws<ArgumentNullException>(() => And(null!, right));

        // assert
        Assert.Equal("left", exception.ParamName);
    }

    [Fact]
    public void And_ShouldReturnPassedResult_WhenBothLeftAndRightReturnPassedResult()
    {
        // arrange
        var left = CreateRule(alwaysTruePredicate);
        var right = CreateRule(alwaysTruePredicate);
        var rule = And(left, right);

        // act
        var result = rule(0);

        // assert
        Assert.IsType<RulePassed>(result);
    }

    [Fact]
    public void And_ShouldReturnFailedResult_WhenLeftReturnsFailedResult()
    {
        // arrange
        var left = CreateRule(alwaysFalsePredicate, code: "left", message: "left-failed");
        var right = CreateRule(alwaysTruePredicate);
        var rule = And(left, right);

        // act
        var result = rule(0);

        // assert
        var failed = Assert.IsType<RuleFailed>(result);
        Assert.Equal("left", failed.Code);
        Assert.Equal("left-failed", failed.Message);
    }

    [Fact]
    public void And_ShouldReturnFailedResult_WhenRightReturnsFailedResult()
    {
        // arrange
        var left = CreateRule(alwaysTruePredicate);
        var right = CreateRule(alwaysFalsePredicate, code: "right", message: "right-failed");
        var rule = And(left, right);

        // act
        var result = rule(0);

        // assert
        var failed = Assert.IsType<RuleFailed>(result);
        Assert.Equal("right", failed.Code);
        Assert.Equal("right-failed", failed.Message);
    }

    [Fact]
    public void Or_ShouldThrowArgumentNullException_WhenRightIsNull()
    {
        // arrange
        var left = CreateRule(alwaysTruePredicate);

        // act
        var exception = Assert.Throws<ArgumentNullException>(() => Or(left, null!));

        // assert
        Assert.Equal("right", exception.ParamName);
    }

    [Fact]
    public void Or_ShouldThrowArgumentNullException_WhenLeftIsNull()
    {
        // arrange
        var right = CreateRule(alwaysTruePredicate);

        // act
        var exception = Assert.Throws<ArgumentNullException>(() => Or(null!, right));

        // assert
        Assert.Equal("left", exception.ParamName);
    }
}
