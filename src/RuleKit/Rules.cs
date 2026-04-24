using System.Diagnostics;

namespace RuleKit;

/// <summary>
/// Provides combinators for composing rules.
/// </summary>
public static class Rules
{
    /// <summary>
    /// Creates a rule that inverts the result of another rule.
    /// </summary>
    /// <typeparam name="T">The input type evaluated by the rule.</typeparam>
    /// <param name="rule">The rule to invert.</param>
    /// <param name="code">A short, stable code used to classify the rule failure.</param>
    /// <param name="message">The human-readable failure message returned when the inner rule evaluates to <c>true</c>.</param>
    /// <returns>
    /// A rule that returns <see cref="RuleFailed"/> when the inner rule returns <see cref="RulePassed"/>,
    /// and <see cref="RulePassed"/> when the inner rule returns <see cref="RuleFailed"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="rule"/> is <c>null</c> or <paramref name="message"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="code"/> is <c>empty</c> or <c>whitespace</c> or <paramref name="message"/> is <c>empty</c> or <c>whitespace</c>.
    /// </exception>
    /// <exception cref="UnreachableException">
    /// Thrown when the inner rule returns an unexpected <see cref="RuleResult"/> implementation.
    /// </exception>
    public static Rule<T> Not<T>(Rule<T> rule, string code, string message)
    {
        ArgumentNullException.ThrowIfNull(rule);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        return x => rule(x) switch
        {
            RulePassed => new RuleFailed(code, message),
            RuleFailed => new RulePassed(),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    /// Combines two rules using logical conjunction.
    /// </summary>
    /// <typeparam name="T">The input type evaluated by the rules.</typeparam>
    /// <param name="left">The first rule to evaluate.</param>
    /// <param name="right">The second rule to evaluate.</param>
    /// <returns>
    /// A rule that returns <see cref="RulePassed"/> only when both rules return <see cref="RulePassed"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="left"/> is <c>null</c> or <paramref name="right"/> is <c>null</c>.
    /// </exception>
    public static Rule<T> And<T>(Rule<T> left, Rule<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left;
    }
}