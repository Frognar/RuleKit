namespace RuleKit;

/// <summary>
/// Represents the result of evaluating a rule.
/// </summary>
/// <param name="IsValid">A value indicating whether the rule passed.</param>
/// <param name="Message">The human-readable message describing the rule outcome.</param>
public sealed record RuleResult(bool IsValid, string? Message);
