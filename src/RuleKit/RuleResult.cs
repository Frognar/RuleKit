namespace RuleKit;

/// <summary>
/// Represents the result of evaluating a rule.
/// </summary>
/// <param name="IsValid">A value indicating whether the rule passed.</param>
public sealed record RuleResult(bool IsValid);
