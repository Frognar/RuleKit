namespace RuleKit;

/// <summary>
/// Represents the result of evaluating a rule.
/// </summary>
public abstract record RuleResult;

/// <summary>
/// Represents the valid result of evaluating a rule.
/// </summary>
public sealed record RulePassed : RuleResult;

/// <summary>
/// Represents the invalid result of evaluating a rule.
/// </summary>
/// <param name="Code">A short, stable code used to classify the rule failure.</param>
/// <param name="Message">The human-readable failure message returned when the predicate evaluates to <c>false</c>.</param>
public sealed record RuleFailed(string Code, string Message) : RuleResult;
