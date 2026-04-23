namespace RuleKit;

/// <summary>
/// Represents a pure rule that evaluates input data and returns a <see cref="RuleResult"/>.
/// </summary>
/// <typeparam name="T">The input type evaluated by the rule.</typeparam>
/// <param name="input">The input value to evaluate.</param>
/// <returns>A <see cref="RuleResult"/> describing whether the input satisfies the rule.</returns>
public delegate RuleResult Rule<in T>(T input);

/// <summary>
/// Provides factory methods for creating rules.
/// </summary>
public static class Rule
{
    /// <summary>
    /// Creates a rule from a predicate function.
    /// </summary>
    /// <typeparam name="T">The input type evaluated by the rule.</typeparam>
    /// <param name="predicate">The predicate used to evaluate the input.</param>
    /// <param name="message">The human-readable message describing the rule outcome.</param>
    /// <returns>A rule that returns a valid result when the predicate evaluates to <c>true</c>; otherwise, an invalid result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="predicate"/> is <c>null</c>.
    /// </exception>
    public static Rule<T> FromPredicate<T>(Func<T, bool> predicate, string message)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(message);
        return x => predicate(x) ? new RuleResult(true, null) : new RuleResult(false, message);
    }
}
