namespace RuleKit;

/// <summary>
/// Represents the result of evaluating a rule.
/// </summary>
public abstract record RuleResult;

/// <summary>
/// Represents the valid result of evaluating a rule.
/// </summary>
public sealed record ValidRuleResult  : RuleResult;

/// <summary>
/// Represents the invalid result of evaluating a rule.
/// </summary>
/// <param name="Message">The human-readable failure message returned when the predicate evaluates to <c>false</c>.</param>
public sealed record InvalidRuleResult(string Message) : RuleResult;
